# dotnet-jwt

When the server validates the user’s credentials and confirms that the user is valid, it’s going to send an encoded JWT to the client. JSON web token is basically a JavaScript object that can contain some attributes of the logged-in user. It can contain a username, user subject, user roles or some other useful information.

At the client side, we store the JWT in browser’s local storage to remember the user’s login session. We may also use the information from the JWT to enhance the security of our application as well.


Configuring JWT Authentication
------------------------------
	public void ConfigureServices(IServiceCollection services)
	{
	    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	    .AddJwtBearer(options =>
	    {
		options.TokenValidationParameters = new TokenValidationParameters
		{
		    ValidateIssuer = true,
		    ValidateAudience = true,
		    ValidateLifetime = true,
		    ValidateIssuerSigningKey = true,

		    ValidIssuer = "http://localhost:5000",
		    ValidAudience = "http://localhost:5000",
		    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
		};
	    });
	}

-------------
Add the app.UseAuthentication() in the Configure method:

	 public void Configure(IApplicationBuilder app, IHostingEnvironment env)
	 {
	    // ...
	    app.UseAuthentication();
	    // ...
	}

And that’s all we need to configure the JWT authentication in ASP.NET Core.

Securing API Endpoints
----------------------
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		// GET api/values
		[HttpGet,Authorize]
		public IEnumerable<string> Get()
		{
			return new string[] { "John Doe", "Jane Doe" };
		}

	}
 
Authorize attribute on top of the GET method restricts the access to only authorized users. Only users who are logged-in can access the list of customers. Therefore, this time if you make a request to http://localhost:5000/api/customers from the browser’s address bar, instead of getting a list of customers you are going to get a 401 Not Authorized response.


Adding Login Endpoint
---------------------

	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
	    // GET api/values
	    [HttpPost, Route("login")]
	    public IActionResult Login([FromBody]LoginModel user)
	    {
		if (user == null)
		{
		    return BadRequest("Invalid client request");
		}

		if (user.UserName == "johndoe" && user.Password == "def@123")
		{
		    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
		    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

		    var tokeOptions = new JwtSecurityToken(
			issuer: "http://localhost:5000",
			audience: "http://localhost:5000",
			claims: new List<Claim>(),
			expires: DateTime.Now.AddMinutes(5),
			signingCredentials: signinCredentials
		    );

		    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
		    return Ok(new { Token = tokenString });
		}
		else
		{
		    return Unauthorized();
		}
	    }
	}


Here comes the interesting part.

The first two steps are the standard steps that you don’t need to worry about. The third step is the one that we are interested in. In the third step we are creating the JwtSecurityToken object with some important parameters:

    Issuer: The first parameter is a simple string representing the name of the web server that issues the token
    Audience: The second parameter is a string value representing valid recipients
    Claims: The third argument is a list of user roles, for example, the user can be an admin, manager or author (we are going to add roles in the next post)
    Expires: The fifth argument is DateTime object that represents the date and time after which the token expires

Finally, we create a string representation of JWT by calling the WriteToken method on JwtSecurityTokenHandler. Finally, we are returning JWT in a response. As a response, we have created an anonymous object that contains only the Token property.

