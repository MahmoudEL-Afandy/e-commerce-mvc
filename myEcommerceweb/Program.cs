using Microsoft.EntityFrameworkCore;
using myEcommerceDataAccess.Data;
using myEcommerceDataAccess.RepositoriesImplementation;
using myEcommerceEntities.Models;
using myEcommerceEntities.Repositories;
using Microsoft.AspNetCore.Identity;
using myEcommerceUtilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Stripe;
using myEcommerceDataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. this to active MVC
builder.Services.AddControllersWithViews();

// we should add this active the MVC.Razor.RuntimeCompilation 
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// we should add it to make the system connect to the DB 
builder.Services.AddDbContext<ApplicationDbContext>(
    options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

// i inject it to make stripeData take data from Stripe section in Json file
builder.Services.Configure<StripeData>(builder.Configuration.GetSection("Stripe"));



// after i add the identity scafolded item pages for identityDbContext the scafolding will add it here . 
// i should to change it from true to false (RequireConfirmedAccount = false)
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();
// after i add role i should change it 
// before i add default ui 
// builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddDefaultUI()
//   .AddEntityFrameworkStores<ApplicationDbContext>();




// after i adddefaultui if the user request page unauthorize for you 
// before i create the system enable lockout to accounts by the admin or when user logging in after many of attempts unsuccessful 

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddDefaultUI()
//    .AddEntityFrameworkStores<ApplicationDbContext>();
// After i create the system enable lockout to accounts by the admin or when user logging in after many of attempts unsuccessful 
// if the user try for 5 attempt failed login the system will lockout for 5 hours 
builder.Services.AddIdentity<IdentityUser, IdentityRole>
    (option=>option.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromHours(4))
    .AddDefaultTokenProviders().AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddSingleton<IEmailSender, EmailSender>();

// we should add it to active the repository patter (unit of work );
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
// i should add this for the implemet DbInitializer 
builder.Services.AddScoped<IDbInitializer,DbInitializer>();

// session Definition :  a session is a way to store information about a user across multiple pages or visits to a website.
// Sessions are crucial for maintaining stateful interactions with users
// and are commonly used to store user-specific data such as login credentials, shopping cart items, preferences, and more.


// i need to get count cart and display it in all pages for the user, and to do it we should use session 
// the following statement for implement session in .net 
// so i need save in the session data about shoppingCart 
// after i implement the session any thing i need to save in the session i can add it in session 

// i will have one example (ShoppingCart) , will save in the session  
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// to map the url with the system views and controlers 
app.UseHttpsRedirection();
// to make the static files such as CSS and JS work in this project the system can understand it 
app.UseStaticFiles();
// this for the system routing 
app.UseRouting();
// this to implement the stripe in my system after i installed stripe.net package in my system
// after i ended from it i should go to the view and create some of implementation for stripe in this page 
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();

// i should call the  SeedInDb () this method to initialize the data 
SeedInDb();

// this to make the system can authorize the users and define the permisions between users and this must be after authontication 
app.UseAuthorization();
// add middleware for session 
app.UseSession();
//app.UseAuthentication();
app.MapRazorPages();

app.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");



app.MapControllerRoute(

    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

// this the first and i chaged it to make the system routing to cUstomer home page 

//app.MapControllerRoute(

//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();

// i will create this method to execute the DbInitializer 

void SeedInDb ()
{
    using (var scope = app.Services.CreateScope())
    {
        var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        DbInitializer.Initialize();
    }
}