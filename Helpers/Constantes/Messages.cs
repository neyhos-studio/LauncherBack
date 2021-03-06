﻿namespace LauncherBack.Helpers
{
    public class Messages
    {
        #region CONNEXION
            public const string COMPTE_INTROUVABLE = "Compte introuvable";
            public const string COMPTE_BANNI = "Tentative de connexion avec un compte banni ...";
        #endregion

        #region INSCRIPTION
            public const string INSCRIPTION_FAILED = "Erreur durant l'inscription";
            public const string INSCRIPTION_OK = "Inscription réussie !";
            public const string INSCRIPTION_EMAIL_EXIST = "Une adresse Email similaire existe déjà dans notre base de données";
            public const string INSCRIPTION_PASSWORD_COURT = "Votre mot de passe doit contenir 8 caractères ou plus";
            public const string INSCRIPTION_PSEUDO_LONG = "Votre pseudo ne doit pas contenir plus de 16 caractères";
            public const string INSCRIPTION_PSEUDO_MOTS_INTERDIT = "Votre pseudo comporte des mots interdits";
            public const string INSCRIPTION_EMAIL_INVALID = "Votre adresse Email n'est pas une adresse valide";
            public const string INSCRIPTION_PSEUDO_2_ESPACES = "Votre pseudo ne peux contenir plus de 2 espaces d'affilés";
            public const string INSCRIPTION_PSEUDO_2_TIRETS = "Votre pseudo ne peux contenir plus de 2 tirets d'affilés";
        #endregion

        #region CONFIGURATION
            public const string CONFIGURATION_MOT_INTERDIT_OK = "Mot interdit ajouté à la liste !";
            public const string CONFIGURATION_MOT_INTERDIT_NOK = "Mot interdit non ajouté à la liste";
        #endregion

        #region BDD
            public const string BDD_CANNOT_CONNECT_SERVER = "Cannot connect to server.  Contact administrator";
            public const string BDD_INVALID_USERNAME_OR_PASSWORD = "Invalid username/password, please try again";
            public const string BDD_ERREUR_CONNEXION_BDD = "Erreur de connexion à la BDD";
        #endregion

        #region MESSAGES SYSTEM
            public const string CONNEXION_IMPOSSIBLE = "Erreur système, contactez un administrateur !";
        #endregion
    }
}
