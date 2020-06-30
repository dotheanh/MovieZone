using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieZone.Data;
using MovieZone.Models;
// using System.Web;
using Microsoft.AspNetCore.Http;


namespace MovieZone.Controllers
{
    public class UserController : Controller
    {
        private readonly MovieContext _context;

        public UserController(MovieContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return PartialView();
        }

        public IActionResult Register()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = _context.Users.FirstOrDefault(s => s.UserName == _user.UserName);
                if (check == null)
                {
                    // _user.Password = GetMD5(_user.Password);                             // chưa mã hóa password
                    // _context.Configuration.ValidateOnSaveEnabled = false;                //// ?????????
                    _context.Users.Add(_user);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "UserName already exists";
                    return View();
                }
            }
            return View();


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password="NOOO")///////////////chưa truyền password
        {
            if (ModelState.IsValid)
            {
                /////////////// var f_password = GetMD5(password);  
                var f_password = password;                             // chưa mã hóa password
                // var data = _context.Users.Where(s => s.UserName.Equals(username) && s.Password.Equals(f_password)).ToList();
                var data = _context.Users.Where(s => s.UserName.Equals(username)).ToList();///////////////chưa truyền password
                if (data.Count() > 0)
                {
                    //add session
                    //Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    HttpContext.Session.SetString("UserName", data.FirstOrDefault().UserName.ToString());
                    HttpContext.Session.SetString("idUser", data.FirstOrDefault().Id.ToString());
                    // Session["UserName"] = data.FirstOrDefault().UserName;
                    // Session["idUser"] = data.FirstOrDefault().Id;
                    return RedirectToAction("Index");
                }   
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();//remove session
            return RedirectToAction("Login");
        }


        // GET: User
        public async Task<IActionResult> Index() // nếu đã đăng nhập rồi thì mới render ra trang index, ngược lại redirect về trang login
        {
            if (HttpContext.Session.GetString("idUser") != null)
            {
                return View(await _context.Users.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,FullName")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,FullName")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
