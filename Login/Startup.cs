using Login.Lib.Enumerators;
using Login.Lib.i18n;
using Login.Services;
using Login.Services.Interfaces;
using Login.Setups;
using Login.Setups.Interfaces;
using Login.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDevPack.Identity;
using NetDevPack.Identity.Jwt;
using System.Threading.Tasks;

namespace Login
{
    public class Startup
    {
        //private readonly IRoleSetup _roleSetup;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //_roleSetup = roleSetup;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddIdentityEntityFrameworkContextConfiguration(options =>
                                                                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                                                                    b => b.MigrationsAssembly(GetType().Namespace)));

            services.AddJwtConfiguration(Configuration, "AppSettings");

            services.AddIdentityConfiguration().AddErrorDescriber<IdentityMessagePtBr>();

            //Inject Dependency
            services.AddTransient<IRoleSetup, RoleSetup>();
            services.AddTransient<IClaimSetup, ClaimSetup>();
            services.AddTransient<AppJwtSettings, AppJwtSettings>();
            services.AddTransient<IUserService, UserService>();

            var serviceProvider = services.BuildServiceProvider();

            //Configure master user
            var userService = serviceProvider.GetService<IUserService>();
            this.ConfigureMasterUser(userService);

            //Configure roles
            var roleSetup = serviceProvider.GetService<IRoleSetup>();
            this.ConfigureRole(roleSetup);

            //Swagger
            SwaggerConfiguration.Config(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
                options.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Login 2DTorres v1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureMasterUser(IUserService userService)
        {
            var masterUser = new Models.RegisterUserModel() { Email = "diegodevtorres@gmail.com",
                                                              EmailConfirmed = true,
                                                              UserName = "mestre",
                                                              Password = "Admin@192014*",
                                                              ConfirmPassword = "Admin@192014*",
                                                              FirstName = "Diego",
                                                              LastName = "Alves Torres"
            };
            var result = Task.Run(async () => await userService.CreateUser(masterUser, true)).Result;
        }

        private void ConfigureRole(IRoleSetup roleSetup)
        {
            var result = Task.Run(async () => await roleSetup.AddRole(RoleEnum.Admin.GetValue(), CustomClaimTypesEnum.Administrator, PermissionEnum.Manager)).Result;
            if (result.Succeeded)
            {
                var resultUser = Task.Run(async () => await roleSetup.AddUserRole("mestre", RoleEnum.Admin.GetValue())).Result;
            }

            var resultRoleBeautySalon = Task.Run(async () => await roleSetup.AddRoleWithBasicPermissions(RoleEnum.BeautySalon.GetValue())).Result;
            if (resultRoleBeautySalon.Succeeded)
            {
                var resultUser = Task.Run(async () => await roleSetup.AddUserRole("mestre", RoleEnum.BeautySalon.GetValue())).Result;
            }
        }
    }
}
