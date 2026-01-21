# Configuration Zitadel pour GestMatch

## Étape 1 : Créer un compte Zitadel

1. Allez sur https://zitadel.cloud
2. Cliquez sur **"Start for free"**
3. Créez votre compte (gratuit jusqu'à 25,000 utilisateurs actifs/mois)
4. Confirmez votre email

## Étape 2 : Créer une nouvelle instance

1. Dans le dashboard Zitadel, cliquez sur **"Create new instance"**
2. Nom : `gestmatch` (ou votre choix)
3. Notez l'URL de votre instance : `https://gestmatch-xxxxx.zitadel.cloud`
4. Cette URL sera votre `ZITADEL_AUTHORITY`

## Étape 3 : Créer un projet

1. Dans votre instance, allez dans **"Projects"**
2. Cliquez sur **"+ New"**
3. Nom du projet : `GestMatch`
4. Type de projet : **"Web/Mobile Application"**
5. Cliquez sur **"Continue"**

## Étape 4 : Créer les rôles

1. Dans votre projet, allez dans l'onglet **"Roles"**
2. Créez 3 rôles :
   - **Admin** : Administration complète
     - Display Name: `Administrator`
     - Key: `Admin`
   - **MatchManager** : Gestion des matchs et tickets
     - Display Name: `Match Manager`
     - Key: `MatchManager`
   - **User** : Utilisateur standard (achat tickets)
     - Display Name: `User`
     - Key: `User`

## Étape 5 : Créer l'application API

1. Dans votre projet, allez dans l'onglet **"Applications"**
2. Cliquez sur **"+ New"**
3. Configuration :
   - **Name** : `GestMatch API`
   - **Type** : **API**
4. Cliquez sur **"Continue"**
5. **Authentication Method** : Sélectionnez **"Basic"** (Client ID + Secret)
6. Cliquez sur **"Continue"**

### Notez les credentials

Après création, Zitadel affichera :
- **Client ID** : Copiez-le dans `.env` → `ZITADEL_CLIENT_ID`
- **Client Secret** : Copiez-le dans `.env` → `ZITADEL_CLIENT_SECRET`

⚠️ **Important** : Le Client Secret ne s'affiche qu'une seule fois !

## Étape 6 : Créer l'application Mobile (MAUI)

1. Toujours dans **"Applications"**, cliquez sur **"+ New"**
2. Configuration :
   - **Name** : `GestMatch Mobile`
   - **Type** : **Native** (ou **User Agent** pour MAUI)
3. Cliquez sur **"Continue"**
4. **Redirect URIs** :
   - `gestmatch://callback` (pour MAUI)
   - `http://localhost:5000/signin-oidc` (pour tests locaux)
5. **Post Logout Redirect URIs** :
   - `gestmatch://logout`
6. **Grant Types** :
   - ✅ Authorization Code
   - ✅ Refresh Token
7. Notez le **Client ID** de l'app mobile

## Étape 7 : Configurer le fichier .env

Mettez à jour le fichier `.env` à la racine du projet :

```env
# Zitadel Configuration
ZITADEL_AUTHORITY=https://gestmatch-xxxxx.zitadel.cloud
ZITADEL_AUDIENCE=<votre-project-id>
ZITADEL_CLIENT_ID=<api-client-id>
ZITADEL_CLIENT_SECRET=<api-client-secret>
```

Pour trouver le **Project ID** (AUDIENCE) :
1. Dans Zitadel, allez dans votre projet
2. L'ID du projet est dans l'URL : `projects/<PROJECT_ID>`
3. Ou dans les détails du projet

## Étape 8 : Créer des utilisateurs de test

1. Dans Zitadel, allez dans **"Users"**
2. Créez 3 utilisateurs pour tester :

### Admin
- **Email** : `admin@gestmatch.sn`
- **First Name** : Admin
- **Last Name** : GestMatch
- **Username** : admin
- Ajoutez le rôle **Admin** dans le projet GestMatch

### Match Manager
- **Email** : `manager@gestmatch.sn`
- **First Name** : Mamadou
- **Last Name** : Diallo
- **Username** : mamadou.diallo
- Ajoutez le rôle **MatchManager** dans le projet GestMatch

### User Standard
- **Email** : `user@gestmatch.sn`
- **First Name** : Fatou
- **Last Name** : Sow
- **Username** : fatou.sow
- Ajoutez le rôle **User** dans le projet GestMatch

## Étape 9 : Redémarrer l'application

```powershell
# Arrêter les containers
docker-compose down

# Redémarrer avec les nouvelles variables
docker-compose up -d --build

# Vérifier les logs
docker logs gestmatch-api --tail 50
```

## Étape 10 : Tester l'authentification

### Avec Swagger UI (http://localhost:5000/swagger)

1. Cliquez sur le bouton **"Authorize"** (cadenas vert)
2. Vous serez redirigé vers Zitadel pour vous connecter
3. Utilisez un des comptes créés (ex: `manager@gestmatch.sn`)
4. Après login, vous serez redirigé vers Swagger avec un token JWT
5. Testez un endpoint protégé comme `POST /api/matches`

### Avec curl/PowerShell

```powershell
# Obtenir un token (Resource Owner Password Flow - DEV uniquement)
$body = @{
    grant_type = "password"
    client_id = "<ZITADEL_CLIENT_ID>"
    client_secret = "<ZITADEL_CLIENT_SECRET>"
    username = "manager@gestmatch.sn"
    password = "<mot-de-passe>"
    scope = "openid profile email"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://gestmatch-xxxxx.zitadel.cloud/oauth/v2/token" `
    -Method POST `
    -Body $body `
    -ContentType "application/json"

$token = $response.access_token

# Utiliser le token pour appeler l'API
$headers = @{
    Authorization = "Bearer $token"
}

Invoke-RestMethod -Uri "http://localhost:5000/api/matches" `
    -Method GET `
    -Headers $headers
```

## Étape 11 : Configurer les claims personnalisés (optionnel)

Pour ajouter des informations supplémentaires dans le token JWT :

1. Dans Zitadel, allez dans **"Actions"**
2. Créez une action de type **"Complement Token"**
3. Code exemple pour ajouter le rôle :

```javascript
function complementToken(ctx, api) {
  if (ctx.v1.user.grants) {
    const roles = ctx.v1.user.grants
      .filter(g => g.projectId === '<PROJECT_ID>')
      .flatMap(g => g.roles);
    
    if (roles.length > 0) {
      api.v1.setClaim('role', roles[0]); // Premier rôle
      api.v1.setClaim('roles', roles);   // Tous les rôles
    }
  }
}
```

4. Activez l'action dans votre projet

## Dépannage

### Les variables d'environnement ne sont pas chargées

Vérifiez que le fichier `.env` est à la racine du projet et que `docker-compose.yml` utilise `env_file: .env`.

### Token invalide ou expiré

- Vérifiez que `ZITADEL_AUTHORITY` est correct (sans `/` à la fin)
- Vérifiez que l'heure système est synchronisée
- Les tokens Zitadel expirent après 12h par défaut

### Rôle non reconnu

Vérifiez que :
- Le rôle est bien créé dans le projet Zitadel
- L'utilisateur a le rôle assigné
- Le claim `role` est présent dans le token (utilisez https://jwt.io pour décoder)

## Ressources

- Documentation Zitadel : https://zitadel.com/docs
- API Reference : https://zitadel.com/docs/apis/introduction
- OIDC/OAuth2 : https://zitadel.com/docs/guides/integrate/login
- GitHub Zitadel : https://github.com/zitadel/zitadel
