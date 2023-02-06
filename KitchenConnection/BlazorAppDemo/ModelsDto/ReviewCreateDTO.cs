using KitchenConnection.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppDemo.ModelsDto { 
    
        public class ReviewDTO
        {
            public Guid Id { get; set; }
            public UserDTO User { get; set; }
            public double Rating { get; set; }
            public string? Message { get; set; }
        
    }
}
