using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtMarketPlaceAPI.Models
{
    public enum StatutCommande
    {
        EnAttente,
        EnPreparation,
        Expediee,
        Livree,
        Annulee,
    }

    public enum StatutLivraison
{
    EnAttenteRetrait,
    EnTransit,
    Livre,
    Distribuee
    }

}