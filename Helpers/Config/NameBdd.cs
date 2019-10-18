using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherBack.Helpers.Config
{
    public class NameBdd
    {
        public const string PREFIX = "NS";
        public const string PREFIX_NAME_FIELD = "_";
        //string.format
        #region NS_ACCOUNTS
            private const string ENTITY_ACCOUNT = "ACCOUNT";
            public static readonly string NAME_TABLE_ACCOUNT = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_ACCOUNT);
            public static readonly string NAME_FIELD_ACCOUNT_ID = String.Format("{0}{1}{2}", ENTITY_ACCOUNT, PREFIX_NAME_FIELD, "ID");
            public static readonly string NAME_FIELD_ACCOUNT_EMAIL = String.Format("{0}{1}{2}", ENTITY_ACCOUNT, PREFIX_NAME_FIELD, "EMAIL");
            public static readonly string NAME_FIELD_ACCOUNT_PASSWORD = String.Format("{0}{1}{2}", ENTITY_ACCOUNT, PREFIX_NAME_FIELD, "PASSWORD");
        #endregion

        #region NS_UTILISATEURS
            private const string ENTITY_UTILISATEUR = "UTILISATEUR";
            public static readonly string NAME_TABLE_UTILISATEUR = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_UTILISATEUR);
            public static readonly string NAME_FIELD_UTILISATEUR_ID = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR, PREFIX_NAME_FIELD, "ID");
            public static readonly string NAME_FIELD_UTILISATEUR_ID_ACCOUNT = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR, PREFIX_NAME_FIELD, "ID_ACCOUNT");
            public static readonly string NAME_FIELD_UTILISATEUR_PSEUDO = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR, PREFIX_NAME_FIELD, "PSEUDO");
            public static readonly string NAME_FIELD_UTILISATEUR_ETAT = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR, PREFIX_NAME_FIELD, "ETAT");
        #endregion

        #region NS_ETATS_UTILISATEUR
            private const string ENTITY_ETAT_UTILISATEUR = "ETAT_UTILISATEUR";
            public static readonly string NAME_TABLE_ETAT_UTILISATEUR = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_ETAT_UTILISATEUR);
            public static readonly string NAME_FIELD_ETAT_UTILISATEUR_ID = String.Format("{0}{1}{2}", ENTITY_ETAT_UTILISATEUR, PREFIX_NAME_FIELD, "ID");
            public static readonly string NAME_FIELD_ETAT_UTILISATEUR_LIBELLE = String.Format("{0}{1}{2}", ENTITY_ETAT_UTILISATEUR, PREFIX_NAME_FIELD, "LIBELLE");
            public static readonly string NAME_FIELD_ETAT_UTILISATEUR_COLOR = String.Format("{0}{1}{2}", ENTITY_ETAT_UTILISATEUR, PREFIX_NAME_FIELD, "COLOR");
        #endregion

        #region NS_TOKENS
            private const string ENTITY_TOKEN = "TOKEN";
            public static readonly string NAME_TABLE_TOKEN = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_TOKEN);
            public static readonly string NAME_FIELD_TOKEN_ID = String.Format("{0}{1}{2}", ENTITY_TOKEN, PREFIX_NAME_FIELD, "ID");
            public static readonly string NAME_FIELD_TOKEN_ACCOUNT_ID = String.Format("{0}{1}{2}", ENTITY_TOKEN, PREFIX_NAME_FIELD, "ACCOUNT_ID");
            public static readonly string NAME_FIELD_TOKEN_TOKEN_SERVER = String.Format("{0}{1}{2}", ENTITY_TOKEN, PREFIX_NAME_FIELD, "TOKEN_SERVER");
            public static readonly string NAME_FIELD_TOKEN_TOKEN_CLIENT = String.Format("{0}{1}{2}", ENTITY_TOKEN, PREFIX_NAME_FIELD, "TOKEN_CLIENT");
        #endregion

        #region NS_BANNISSEMENTS
            private const string ENTITY_BANNISSEMENT = "BANNISSEMENT";
            public static readonly string NAME_TABLE_BANNISSEMENT = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_BANNISSEMENT);
            public static readonly string NAME_FIELD_BANNISSEMENT_ID = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "ID");
            public static readonly string NAME_FIELD_BANNISSEMENT_ACCOUNT = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "ID_ACCOUNT");
            public static readonly string NAME_FIELD_BANNISSEMENT_DATE_DEBUT = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "DATE_DEBUT");
            public static readonly string NAME_FIELD_BANNISSEMENT_DUREE = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "DUREE");
            public static readonly string NAME_FIELD_BANNISSEMENT_DATE_FIN = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "DATE_FIN");
            public static readonly string NAME_FIELD_BANNISSEMENT_RAISON = String.Format("{0}{1}{2}", ENTITY_BANNISSEMENT, PREFIX_NAME_FIELD, "RAISON");
        #endregion

        #region NS_JEUX
        private const string ENTITY_JEU = "JEU";
            public static readonly string NAME_TABLE_JEU = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_JEU);
            public static readonly string NAME_FIELD_JEU_ID = String.Format("{0}{1}{2}", ENTITY_JEU, PREFIX_NAME_FIELD, "ID");
            public static readonly string NAME_FIELD_JEU_NAME = String.Format("{0}{1}{2}", ENTITY_JEU, PREFIX_NAME_FIELD, "NAME");
        #endregion

        #region NS_UTILISATEUR_JEU
            private const string ENTITY_UTILISATEUR_JEU = "UTILISATEUR_JEU";
            public static readonly string NAME_TABLE_UTILISATEUR_JEU = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_UTILISATEUR_JEU);
            public static readonly string NAME_FIELD_UTILISATEUR_JEU_ID = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR_JEU, PREFIX_NAME_FIELD, "ID");
            public static readonly string NAME_FIELD_UTILISATEUR_JEU_JEU = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR_JEU, PREFIX_NAME_FIELD, "JEU");
            public static readonly string NAME_FIELD_UTILISATEUR_JEU_UTILISATEUR = String.Format("{0}{1}{2}", ENTITY_UTILISATEUR_JEU, PREFIX_NAME_FIELD, "UTILISATEUR");
        #endregion

        #region NS_SOCIAL
            private const string ENTITY_SOCIAL = "SOCIAL";
            public static readonly string NAME_TABLE_SOCIAL = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_SOCIAL);
            public static readonly string NAME_FIELD_SOCIAL_ID = String.Format("{0}{1}{2}", ENTITY_SOCIAL, PREFIX_NAME_FIELD, "ID");
            public static readonly string NAME_FIELD_SOCIAL_UTILISATEUR_ID = String.Format("{0}{1}{2}", ENTITY_SOCIAL, PREFIX_NAME_FIELD, "UTILISATEUR_ID");
            public static readonly string NAME_FIELD_SOCIAL_APOURAMI_ID = String.Format("{0}{1}{2}", ENTITY_SOCIAL, PREFIX_NAME_FIELD, "APOURAMI_ID");
        #endregion

        #region NS_MOTS_iNTERDIS
            private const string ENTITY_MOT_INTERDIT = "MOT_INTERDIT";
            public static readonly string NAME_TABLE_MOT_INTERDIT = String.Format("{0}{1}{2}", PREFIX, PREFIX_NAME_FIELD, ENTITY_MOT_INTERDIT);
            public static readonly string NAME_FIELD_MOT_INTERDIT_ID = String.Format("{0}{1}{2}", ENTITY_MOT_INTERDIT, PREFIX_NAME_FIELD, "ID");
            public static readonly string NAME_FIELD_MOT_INTERDIT_LIBELLE = String.Format("{0}{1}{2}", ENTITY_MOT_INTERDIT, PREFIX_NAME_FIELD, "LIBELLE");
        #endregion

    }
}
