using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab8.Models.DataAccess;
using Lab8.Models.ViewModel;
using System.Data;


namespace Lab8.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly StudentRecordsContext _context;

        public EmployeesController(StudentRecordsContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
              return View(await _context.Employees.Include(r=>r.Roles).ToListAsync());
        }
        

        // GET: Employees/Create
        public IActionResult Create()
        {
            var employee = new Employee();
            var roles = _context.Roles.ToList();

            var employeeRoleSelections = new EmployeeRoleSelections(employee, roles);
            return View(employeeRoleSelections);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeRoleSelections employeeRoleSelections)
        {
            // check whether users select any of roles
            if (!employeeRoleSelections.roleSelections.Any(m => m.Selected))
            {
                ModelState.AddModelError("roleSelections", "You must select at least one role!");
                employeeRoleSelections  = new EmployeeRoleSelections(employeeRoleSelections.employee, _context.Roles.ToList());
            }

            // check whether user name has been used by another employee or not
            if (_context.Employees.Any(e => e.UserName == employeeRoleSelections.employee.UserName
                                            && e.Id != employeeRoleSelections.employee.Id))
            {
                ModelState.AddModelError("employee.UserName", "This user name has been used by another employee!");
                employeeRoleSelections = new EmployeeRoleSelections(employeeRoleSelections.employee, _context.Roles.ToList());
            }


            if (ModelState.IsValid)
            {
                foreach(RoleSelection roleSelection in employeeRoleSelections.roleSelections)
                {
                    if (roleSelection.Selected)
                    {
                        
                        Role role = _context.Roles.SingleOrDefault(r => r.Id == roleSelection.role.Id);
                        employeeRoleSelections.employee.Roles.Add(role);
                        
                    }
                }
                
                _context.Add(employeeRoleSelections.employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
                
            }
            return View(employeeRoleSelections);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            //var employee = await _context.Employees.FindAsync(id);
            var employee = await _context.Employees.Include(e => e.Roles).SingleOrDefaultAsync(e => e.Id == id);
            var roles = await _context.Roles.ToListAsync();

            var employeeRoleSelections = new EmployeeRoleSelections(employee, roles);
            if (employeeRoleSelections == null)
            {
                return NotFound();
            }
            //return View(employee);
            return View(employeeRoleSelections);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeRoleSelections employeeRoleSelections)
        {
            // check whether users select any of roles
            if (!employeeRoleSelections.roleSelections.Any(m => m.Selected) )
            {
                ModelState.AddModelError("roleSelections", "You must select at least one role!");
                employeeRoleSelections = new EmployeeRoleSelections(employeeRoleSelections.employee, _context.Roles.ToList());
            }

            // check whether user name has been used by another employee or not
            if(_context.Employees.Any(e => e.UserName == employeeRoleSelections.employee.UserName
                                            && e.Id != employeeRoleSelections.employee.Id))
            {
                ModelState.AddModelError("employee.UserName", "This user name has been used by another employee!");
                employeeRoleSelections = new EmployeeRoleSelections(employeeRoleSelections.employee, _context.Roles.ToList());
            }

            
            if (ModelState.IsValid)
            {
                Employee employee = await _context.Employees.Include(e => e.Roles)
                    .SingleOrDefaultAsync(e => e.Id == employeeRoleSelections.employee.Id);
                employee.Roles.Clear();

                foreach(RoleSelection roleSelection in employeeRoleSelections.roleSelections)
                {
                    if(roleSelection.Selected)
                    {
                        Role role = _context.Roles.SingleOrDefault(r => r.Id == roleSelection.role.Id);
                        employee.Roles.Add(role);
                    }
                }
                // commented out by miku 2022.11.26
                employee.Name = employeeRoleSelections.employee.Name;
                employee.UserName = employeeRoleSelections.employee.UserName;
                employee.Password = employeeRoleSelections.employee.Password;
                
                _context.Update(employee);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            /*
            employeeRoleSelections.CreateRoleSelections(_context.Roles.ToList());
            */
            return View(employeeRoleSelections);
        }


        /*
        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'StudentRecordsContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        private bool EmployeeExists(int id)
        {
          return _context.Employees.Any(e => e.Id == id);
        }
    }
}
