﻿using Lab8.Models.DataAccess;

namespace Lab8.Models.ViewModel
{
    public class RoleSelection
    {
        public Role role { get; set; }

        public bool Selected { get; set;}

        public RoleSelection()
        {
            role = null;
            Selected = false;
        }

        public RoleSelection(Role role, bool selected = false)
        {
            this.role = role;
            Selected = selected;
        }
    }
}
