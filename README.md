# CampusDesk - Student Course Registration System

ASP.NET Core MVC (C#) · Bootstrap 5 · in-memory `static List<Student>` (no database)

## 1. How to run

**Requirements:** .NET 8 SDK (https://dotnet.microsoft.com/download) and an internet connection
(Bootstrap, icons, fonts and jQuery validation load from CDNs).

```bash
cd StudentCourseRegistration
dotnet run
```

Open **http://localhost:5080**. In Visual Studio 2022: double-click `StudentCourseRegistration.sln` (or File -> Open -> Project/Solution) and press **F5** (or click the green Run button).

If you currently have the folder open in Visual Studio:
1. In Solution Explorer, double-click `StudentCourseRegistration.sln` to switch from Folder View to Solution View.
2. Select `StudentCourseRegistration` or `IIS Express` from the debug dropdown at the top.
3. Press **F5** (Debug -> Start Debugging) or **Ctrl+F5** (Start Without Debugging).

If you have a newer SDK only (9 or 10), change `<TargetFramework>net8.0</TargetFramework>`
in `StudentCourseRegistration.csproj` to `net9.0` / `net10.0`.

> Data is kept in memory. Stopping the app clears new registrations and the 5 sample students come back.

## 2. Folder structure

```
StudentCourseRegistration/
├── Program.cs                      App startup + middleware + routing
├── appsettings.json                Logging / host settings
├── Properties/launchSettings.json  Port (5080) and environment
├── Models/
│   ├── Student.cs                  Student model + validation attributes
│   ├── CourseCatalog.cs            Available courses (one shared list)
│   ├── StudentListViewModel.cs     Data for the student list page
│   ├── HomeViewModel.cs            Data for the dashboard
│   └── ErrorViewModel.cs           Data for the error page
├── Data/
│   └── StudentStore.cs             The static List<Student> + safe helper methods
├── Controllers/
│   ├── HomeController.cs           Dashboard + error page
│   └── StudentController.cs        Index, Create (GET/POST), Details
├── Views/
│   ├── _ViewImports.cshtml         Common @using + tag helpers
│   ├── _ViewStart.cshtml           Sets the default layout
│   ├── Home/Index.cshtml           Welcome page + dashboard
│   ├── Student/Index.cshtml        Table, search, course filter
│   ├── Student/Create.cshtml       Registration form
│   ├── Student/Details.cshtml      One student's details
│   ├── Student/NotFound.cshtml     Friendly "invalid ID" page
│   └── Shared/
│       ├── _Layout.cshtml          Navbar, footer, CSS/JS links
│       ├── _Alerts.cshtml          Success / error flash messages
│       ├── _ValidationScriptsPartial.cshtml   Client-side validation scripts
│       └── Error.cshtml            404 / 500 page
└── wwwroot/css/site.css            Custom theme
```

## 3. Important files and methods (for your viva)

| Item | What it does |
|---|---|
| `Program.cs` | `AddControllersWithViews()` registers MVC. `MapControllerRoute` defines the URL pattern `{controller}/{action}/{id?}` and makes `Home/Index` the start page. `UseStatusCodePagesWithReExecute` shows our styled page for 404s. |
| `Student.cs` | The model. `[Required]`, `[StringLength]`, `[EmailAddress]`, `[Range]` are Data Annotations = validation rules. `Semester` is `int?` so an unselected dropdown gives "required" instead of 0. |
| `StudentStore.cs` | Holds the `static List<Student>`. Static = one list shared by all users. `lock` prevents two simultaneous requests corrupting it. |
| `StudentController.Index(searchTerm, course)` | Model binding fills the parameters from the query string. Uses LINQ `Where` + `Contains` for search and `==` for the course filter, then sends a `StudentListViewModel` to the view. |
| `Create()` GET | Returns the empty form. |
| `Create(Student)` POST | `[Bind("Name,Email,Course,Semester")]` = model binding with only the allowed fields (prevents over-posting). Checks duplicate email, then `ModelState.IsValid`. If invalid → `return View(student)` (form re-shown with errors). If valid → add to list, set `TempData`, `RedirectToAction` (Post-Redirect-Get). |
| `[ValidateAntiForgeryToken]` | Protects the form against CSRF; the `<form asp-action>` tag helper adds the hidden token automatically. |
| `Details(int id)` | Looks up the student; if not found returns HTTP 404 with the friendly `NotFound` view. |
| `TempData["SuccessMessage"]` | One-time message that survives a redirect; displayed by `_Alerts.cshtml`. |
| Tag Helpers | `asp-for`, `asp-action`, `asp-route-id`, `asp-validation-for`, `asp-validation-summary`, `asp-items`, `<partial>`. |

**Request flow (registration):** browser GET `/Student/Create` → form shown → user submits (POST) → model binding creates `Student` → validation attributes run → `ModelState.IsValid`? → no: same form with errors / yes: saved → redirect to `/Student/Details/{id}` → success banner.

## 4. Testing steps

| # | Feature | Steps | Expected result |
|---|---|---|---|
| 1 | Home / dashboard | Open `/` | Hero, total students = 5, courses offered = 6, per-course bars, recent registrations |
| 2 | Navigation | Click Dashboard, Students, Register student, brand logo | Each page opens; active menu item is highlighted |
| 3 | Student list | Open `/Student` | Table with 5 sample students, "Showing 5 of 5 students" |
| 4 | Search | Type `priya`, Apply | Only Priya Nair. Try `example.com` → all rows (email match) |
| 5 | Course filter | Choose "B.Tech CSE", Apply | 2 students. Clear button resets |
| 6 | Search + filter | `a` + "B.Tech IT" | Only IT students whose name/email contains `a` |
| 7 | No results | Search `zzzz` | "No students found" message with Clear filters |
| 8 | Required validation | Open `/Student/Create`, press Register with empty form | Red messages under all four fields |
| 9 | StringLength | Name = `Al` | "Name must be between 3 and 60 characters." |
| 10 | EmailAddress | Email = `abc` | "Please enter a valid email address." |
| 11 | Range / dropdown | Leave semester as "Select a semester" | "Please select a semester." |
| 12 | Duplicate email | Use `priya.nair@example.com` | "This email address is already registered." |
| 13 | Successful registration | Name `Ananya Krishnan`, email `ananya@example.com`, B.Tech CSE, Semester 2 | Redirect to Details page with green success message |
| 14 | Total updates | Go to `/` | Total students = 6; Ananya in recent registrations |
| 15 | Details | Click View on any row | Profile card + all details |
| 16 | Invalid ID | Open `/Student/Details/999` and `/Student/Details/abc` | "Student not found" page (HTTP 404), no crash |
| 17 | Unknown URL | Open `/nothing` | Styled "Page not found" |
| 18 | Server-side validation | Browser DevTools → block the 3 jQuery scripts (or disable JS) → submit invalid form | Same errors still appear (server-side check) |
| 19 | Responsive | Resize browser below 768 px | Navbar collapses to a menu; table scrolls horizontally |

## 5. Screenshots to capture for your report

1. `/` - hero + dashboard (full page)
2. `/Student` - list with 5 students
3. `/Student?searchTerm=priya` - search result
4. `/Student?course=B.Tech+CSE` - filtered result
5. `/Student/Create` - empty form
6. `/Student/Create` - after submitting empty form (validation errors visible)
7. `/Student/Details/{id}` right after registering (green success banner)
8. `/Student/Details/999` - error handling
9. Mobile view (DevTools → toggle device toolbar, Ctrl+Shift+M) of the home page

Tip: Chrome → F12 → Ctrl+Shift+P → type "Capture full size screenshot".

## 6. Possible viva questions

- **Why `static List`?** One shared in-memory store, no DB needed; lost on restart. In a real app this would be Entity Framework + SQL.
- **Why Post-Redirect-Get?** Refreshing after POST would otherwise re-submit the form and create duplicates.
- **What does `ModelState.IsValid` check?** All Data Annotation rules plus errors we add with `ModelState.AddModelError`.
- **Why `[Bind]`?** Only listed fields are accepted from the form (stops over-posting of `Id`).
- **Client vs server validation?** jQuery Unobtrusive gives instant feedback from `data-val-*` attributes; server-side validation is the real security check.
- **What is a ViewModel?** A class shaped exactly for one view (e.g. students + search term + total count).
