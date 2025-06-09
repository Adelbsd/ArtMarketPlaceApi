using System.ComponentModel.DataAnnotations;
using ArtMarketPlaceAPI.Models;

namespace ArtMarketPlaceAPI.DTO
{
    public class Register
    {
        [Required(ErrorMessage = "Le nom complet est requis.")]
        public required string NomComplet { get; set; }

        [Required(ErrorMessage = "L'adresse e mail est requise.")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
        public required string Mdp { get; set; }

        [Required(ErrorMessage = "Le role est requis.")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Le rôle n'est pas valide")]
        public UserRole Role { get; set; }
    }
}
