using System.ComponentModel.DataAnnotations;
using backend.Models.Interfaces;

namespace backend.Models
{
    public class User : IUser
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string ImageUrl { get; set; } = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iOTgiIGhlaWdodD0iOTgiIHZpZXdCb3g9IjAgMCA5OCA5OCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPGNpcmNsZSBjeD0iNDkiIGN5PSI0OSIgcj0iNDkiIGZpbGw9IiNGNUY1RjUiLz4KPHBhdGggZD0iTTQ5LjEwMDggNDYuMTAwMUM1Mi40NDIyIDQ2LjEwMDEgNTUuMTUwOSA0My4zOTE0IDU1LjE1MDkgNDAuMDUwMUM1NS4xNTA5IDM2LjcwODcgNTIuNDQyMiAzNCA0OS4xMDA4IDM0QzQ1Ljc1OTUgMzQgNDMuMDUwOCAzNi43MDg3IDQzLjA1MDggNDAuMDUwMUM0My4wNTA4IDQzLjM5MTQgNDUuNzU5NSA0Ni4xMDAxIDQ5LjEwMDggNDYuMTAwMVoiIGZpbGw9IiNBQUFBQUEiLz4KPHBhdGggb3BhY2l0eT0iMC41IiBkPSJNNjEuMjAwMiA1Ny40NDNDNjEuMjAwMiA2MS4yMDIxIDYxLjIwMDIgNjQuMjQ5MyA0OS4xMDAxIDY0LjI0OTNDMzcgNjQuMjQ5MyAzNyA2MS4yMDIxIDM3IDU3LjQ0M0MzNyA1My42ODQgNDIuNDE3NCA1MC42MzY3IDQ5LjEwMDEgNTAuNjM2N0M1NS43ODI4IDUwLjYzNjcgNjEuMjAwMiA1My42ODQgNjEuMjAwMiA1Ny40NDNaIiBmaWxsPSIjQUFBQUFBIi8+Cjwvc3ZnPgo=";
        public string AddressLine1 { get; set; } = "";
        public string AddressLine2 { get; set; } = "";
        public string Gender { get; set; } = "Not Selected";
        public string DoB { get; set; } = "yyyy-mm-dd";
        public string Phone { get; set; } = "+0 00 000 0000";
    }
}