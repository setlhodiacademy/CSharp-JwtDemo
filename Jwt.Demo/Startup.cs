using Jwt.Demo.Common;
using Jwt.Demo.Persistence;
using Jwt.Demo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IUserService, UserService>();
            services.AddDbContext<IJwtDemoContext, JwtDemoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("JwtDemoConn")));

            //TOKEN CONFIG
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        //FIXME: MAKE THIS TRUE
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // REGISTERING POLICIES
            services.AddAuthorization(options =>
            {
                // POSTS RELATED CLAIMS
                options.AddPolicy(ClaimConstants.CANCREATEPOSTS, policy => policy.RequireClaim(ClaimConstants.CANCREATEPOSTS));
                options.AddPolicy(ClaimConstants.CANVIEWPOSTS, policy => policy.RequireClaim(ClaimConstants.CANVIEWPOSTS));
                options.AddPolicy(ClaimConstants.CANDELETEPOSTS, policy => policy.RequireClaim(ClaimConstants.CANDELETEPOSTS));

                // USER RELATED CLAIMS
                options.AddPolicy(ClaimConstants.CANADDUSERS, policy => policy.RequireClaim(ClaimConstants.CANADDUSERS));
                options.AddPolicy(ClaimConstants.CANVIEWUSERS, policy => policy.RequireClaim(ClaimConstants.CANVIEWUSERS));
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
