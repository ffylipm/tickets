using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tickets.API.Common;
using Tickets.Models;
using Tickets.Persistence;

namespace Tickets.API.Service
{
    public class UserService
    {
        private readonly TicketsContext context;
        public UserService(TicketsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<UserDTO>> GetUsers(
            int skip,
            int take,
            string? name,
            string? lastname,
            string? phone,
            string? document,
            string? documentType,
            string? userId,
            bool? active
            )
        {
            var query = context.Users.Select(u => u);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(lastname))
            {
                query = query.Where(u => u.Lastname != null && u.Lastname.Contains(lastname));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(u => u.Phone != null && u.Phone.Contains(phone));
            }

            if (!string.IsNullOrEmpty(document))
            {
                query = query.Where(u => u.Document.Contains(document));
            }

            if (!string.IsNullOrEmpty(documentType))
            {
                query = query.Where(u => u.DocumentType.Contains(documentType));
            }

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(u => u.UserId.Contains(userId));
            }

            if (active.HasValue)
            {
                query = query.Where(u => u.Active == active.Value);
            }

            return await query
                .OrderByDescending(u => u.Name)
                .Skip(skip)
                .Take(take)
                .Select(u => new UserDTO()
                {
                    Name = u.Name,
                    Active = u.Active,
                    Document = u.Document,
                    DocumentType = u.DocumentType,
                    Lastname = u.Lastname,
                    Phone = u.Phone,
                    UserId = u.UserId,
                    Rols = u.UserRols.Where(ur => ur.Active).Select(r => new RolDTO()
                    {
                        Active = r.Active,
                        Description = r.Rol.Description,
                        Name = r.Rol.Name,
                        Virtual = r.Rol.Virtual,
                        RolId = r.RolId,
                        Menus = r.Rol.RolMenus.Where(rm => rm.Active).Select(m => new MenuDTO()
                        {
                            Active = m.Active,
                            Description = m.Menu.Description,
                            Name = m.Menu.Name,
                            MenuId = m.MenuId,
                            Path = m.Menu.Path
                        })

                    })
                }).ToListAsync();
        }

        public async Task<User> GetUser(string userId, bool active = true)
        {
            User? user = await context.Users
            .Include(u => u.UserRols)
            .ThenInclude(r => r.Rol)
            .ThenInclude(r => r.RolMenus)
            .ThenInclude(r => r.Menu)
            .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new CustomException($"No existe el usuario.");
            }

            if (active && !user.Active)
            {
                throw new CustomException($"El usuario está inactivo.");

            }

            return user;
        }

        public async Task<UserDTO> Login(UserDTO login)
        {
            User? user = await GetUser(login.UserId);

            if (user.Password != login.Password)
            {
                throw new CustomException($"La constraseña es incorrecta.");
            }

            login.Rols = user.UserRols
                .Select(r => new RolDTO()
                {
                    RolId = r.Rol.RolId,
                    Active = r.Rol.Active,
                    Description = r.Rol.Description,
                    Name = r.Rol.Name,
                    Menus = r.Rol.RolMenus.Select(r => new MenuDTO()
                    {
                        Name = r.Menu.Name,
                        Description = r.Menu.Description,
                        MenuId = r.MenuId,
                        Active = r.Active,
                        Path = r.Menu.Path
                    })
                }).ToList();

            return login;
        }

        public async Task<UserDTO> AddUser(UserDTO add)
        {
            using (var tx = await context.Database.BeginTransactionAsync())
            {
                User user = new()
                {
                    Active = true,
                    Document = add.Document,
                    DocumentType = add.DocumentType,
                    Lastname = add.Lastname,
                    Name = add.Name,
                    Password = add.Password,
                    Phone = add.Phone,
                    UserId = add.UserId
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                IEnumerable<UserRol> rols = add.Rols.Select(r => new UserRol() 
                {
                    Active = true,
                    RolId = r.RolId,
                    UserId = user.UserId
                });

                context.UserRols.AddRange(rols);
                await context.SaveChangesAsync();

                await tx.CommitAsync();
            }

            return add;
        }

        public async Task<UserDTO> UpdUser(UserDTO upd)
        {
            using (var tx = await context.Database.BeginTransactionAsync())
            {
                User user = await GetUser(upd.UserId, active: false);

                user.Document = upd.Document;
                user.DocumentType = upd.DocumentType;
                user.Lastname = upd.Lastname;
                user.Name = upd.Name;
                user.Phone = upd.Phone;

                if (upd.Active && !user.Active)
                {
                    user.Active = upd.Active;
                }

                foreach (var rol in upd.Rols)
                {
                    UserRol? duplicated = await context.UserRols.FirstOrDefaultAsync(ur => ur.UserId == user.UserId && ur.RolId == rol.RolId);
                    if (duplicated != null)
                    {
                        if (duplicated.Active == rol.Active)
                        {
                            continue;
                        }
                        else
                        {
                            duplicated.Active = rol.Active;
                            context.UserRols.Update(duplicated);
                            await context.SaveChangesAsync();

                            continue;
                        }
                    }

                    UserRol userrol = new()
                    {
                        RolId = rol.RolId,
                        UserId = user.UserId,
                        Active = rol.Active
                    };

                    context.UserRols.Add(userrol);
                    await context.SaveChangesAsync();
                }

                context.Users.Update(user);
                await context.SaveChangesAsync();
                await tx.CommitAsync();
            }

            return upd;
        }

        public async Task<UserDTO> DelUser(UserDTO del)
        {
            User user = await GetUser(del.UserId);

            user.Active = false;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return del;
        }



    }
}
