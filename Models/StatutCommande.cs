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
        Expediée,
        Livrée,
        Annulée,
    }

    public enum StatutLivraison
{
    EnAttenteRetrait,
    EnTransit,
    Livré,
    Distribuée
    }

}