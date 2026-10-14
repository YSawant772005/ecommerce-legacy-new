# Byte 6: Authentication and authorization

**Builds on:** Bytes 3–5

**File(s):** [backend/Ecommerce.Api/Controllers/AuthController.cs](../backend/Ecommerce.Api/Controllers/AuthController.cs), [backend/Ecommerce.Infrastructure/Services/PasswordHasher.cs](../backend/Ecommerce.Infrastructure/Services/PasswordHasher.cs), [backend/Ecommerce.Api/Controllers/AdminController.cs](../backend/Ecommerce.Api/Controllers/AdminController.cs)

**In plain terms:**

Authentication proves who the user is. Authorization decides what that user is allowed to do.

**The code:**

```csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role ?? "Customer")
};
```

```csharp
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
```

**What's happening:**

Login verifies a PBKDF2 password hash and issues a JWT containing identity and role claims. The admin controller requires a valid token with the `Admin` role. Refresh tokens are stored in the database and rotated during refresh.

**Connection to the project:**

The frontend stores tokens and uses route guards, but the backend remains the security boundary.

**Why it matters:**

Client-side guards can be bypassed by calling the API directly. Sensitive operations must remain protected on the server.

**Remember:**

Frontend guards improve navigation; backend authorization enforces security.

**Check yourself:**

What claim does the admin policy use to distinguish administrators?
