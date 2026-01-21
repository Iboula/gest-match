# Configuration Zitadel On-Premise pour GestMatch

Ce guide explique comment configurer Zitadel en version auto-hébergée (on-premise) avec Docker Compose.

## Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    Docker Network                        │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │   Zitadel    │  │  Zitadel DB  │  │   GestMatch  │  │
│  │   :8080      │←─│  PostgreSQL  │  │   Postgres   │  │
│  └──────┬───────┘  └──────────────┘  └──────────────┘  │
│         │                                               │
│         ↓                                               │
│  ┌──────────────┐                                       │
│  │ GestMatch API│                                       │
│  │   :5000      │                                       │
│  └──────────────┘                                       │
└─────────────────────────────────────────────────────────┘
```

## Étape 1 : Démarrer Zitadel

### 1.1 Démarrer tous les services

```powershell
# Démarrer tous les containers
docker-compose up -d

# Vérifier le statut
docker-compose ps

# Suivre les logs de Zitadel (première initialisation = 1-2 min)
docker logs gestmatch-zitadel -f
```

### 1.2 Attendre l'initialisation

Zitadel prend environ 1-2 minutes pour s'initialiser. Attendez le message :
```
INFO[...] ready to serve grpc.port=8080 ...
```

### 1.3 Accéder à la console

Ouvrez votre navigateur : **http://localhost:8080**

**Identifiants par défaut :**
- **Email** : `admin@gestmatch.sn`
- **Password** : `Admin@2026!` (configuré dans `.env` via `ZITADEL_ADMIN_PASSWORD`)

⚠️ **Changez le mot de passe** après la première connexion !

## Étape 2 : Configurer l'organisation

### 2.1 Vérifier l'organisation

Zitadel crée automatiquement l'organisation **"GestMatch"** au premier démarrage.

1. Connectez-vous avec `admin@gestmatch.sn`
2. Vérifiez que vous êtes dans l'organisation **GestMatch**
3. Sinon, cliquez sur le nom de l'organisation en haut à droite pour changer

## Étape 3 : Créer le projet

1. Dans le menu de gauche, cliquez sur **"Projects"**
2. Cliquez sur **"+ New Project"**
3. Configuration :
   - **Name** : `GestMatch`
   - **Project Role Assertion** : ✅ Cochez la case
   - **Project Role Check** : ✅ Cochez la case
4. Cliquez sur **"Continue"**

**Notez le Project ID** affiché dans l'URL : `http://localhost:8080/projects/<PROJECT_ID>`

Ce sera votre **ZITADEL_AUDIENCE** dans le fichier `.env`

## Étape 4 : Créer les rôles

1. Dans votre projet **GestMatch**, allez dans l'onglet **"Roles"**
2. Créez les 3 rôles suivants :

### Rôle Admin
- Cliquez sur **"+ New"**
- **Key** : `Admin`
- **Display Name** : `Administrator`
- **Group** : (laissez vide)
- Cliquez sur **"Save"**

### Rôle MatchManager
- **Key** : `MatchManager`
- **Display Name** : `Match Manager`
- Cliquez sur **"Save"**

### Rôle User
- **Key** : `User`
- **Display Name** : `User`
- Cliquez sur **"Save"**

## Étape 5 : Créer l'application API

1. Dans votre projet **GestMatch**, allez dans l'onglet **"Applications"**
2. Cliquez sur **"+ New"**
3. Configuration :

### 5.1 Type d'application
- **Name** : `GestMatch API`
- **Type** : Sélectionnez **"API"**
- Cliquez sur **"Continue"**

### 5.2 Méthode d'authentification
- **Authentication Method** : **Basic** (Client ID + Secret)
- Cliquez on **"Continue"**

### 5.3 Vérification
- Vérifiez les paramètres
- Cliquez sur **"Create"**

### 5.4 Récupérer les credentials

⚠️ **IMPORTANT** : Copiez ces valeurs immédiatement (le secret ne s'affichera plus) !

**Client ID** : 
```
123456789012345678@gestmatch
```

**Client Secret** :
```
abcdefghijklmnopqrstuvwxyz123456789ABCDEFGHIJKLMNOP
```

## Étape 6 : Créer l'application Mobile (MAUI)

1. Toujours dans **"Applications"**, cliquez sur **"+ New"**
2. Configuration :

### 6.1 Type d'application
- **Name** : `GestMatch Mobile`
- **Type** : **Native** ou **User Agent**
- Cliquez sur **"Continue"**

### 6.2 Redirect URIs
Ajoutez les URIs suivantes :
- `gestmatch://callback` (pour MAUI)
- `http://localhost:5000/signin-oidc` (pour tests avec Swagger)
- `http://localhost/signin-oidc` (pour tests)

### 6.3 Post Logout URIs
- `gestmatch://logout`
- `http://localhost:5000/signout-callback-oidc`

### 6.4 Grant Types
Sélectionnez :
- ✅ **Authorization Code**
- ✅ **Refresh Token**

### 6.5 Application Type
- **Application Type** : **User Agent** (pour MAUI/mobile)
- Cliquez sur **"Continue"** puis **"Create"**

### 6.6 Récupérer le Client ID

**Client ID Mobile** :
```
987654321098765432@gestmatch
```

(Pas de secret pour une app native/mobile)

## Étape 7 : Mettre à jour le fichier .env

Modifiez le fichier `.env` à la racine du projet :

```env
# Zitadel On-Premise Configuration
ZITADEL_MASTERKEY=MasterkeyNeedsToHave32Characters
ZITADEL_DB_PASSWORD=zitadel123
ZITADEL_ADMIN_PASSWORD=VotreNouveauMotDePasse2026!

# Configuration API
ZITADEL_AUTHORITY=http://localhost:8080
ZITADEL_AUDIENCE=<PROJECT_ID>
ZITADEL_CLIENT_ID=<API_CLIENT_ID>
ZITADEL_CLIENT_SECRET=<API_CLIENT_SECRET>

# Database Configuration
POSTGRES_PASSWORD=gestmatch123
PGADMIN_EMAIL=admin@gestmatch.sn
PGADMIN_PASSWORD=admin123
```

Remplacez :
- `<PROJECT_ID>` : ID du projet (de l'étape 3)
- `<API_CLIENT_ID>` : Client ID de l'API (étape 5)
- `<API_CLIENT_SECRET>` : Client Secret de l'API (étape 5)

## Étape 8 : Créer les utilisateurs de test

### 8.1 Créer un utilisateur Admin

1. Dans Zitadel Console, allez dans **"Users"**
2. Cliquez sur **"+ New"**
3. Type : **Human User**
4. Configuration :
   - **Username** : `admin`
   - **First Name** : `Admin`
   - **Last Name** : `GestMatch`
   - **Email** : `admin@gestmatch.local` (l'admin principal est déjà admin@gestmatch.sn)
   - **Phone** : (optionnel)
   - ✅ **Email Verified**
5. Cliquez sur **"Create"**
6. Définissez un mot de passe temporaire

### 8.2 Assigner le rôle Admin

1. Sur la page de l'utilisateur, allez dans **"Authorizations"**
2. Cliquez sur **"+ New"**
3. Sélectionnez le projet **GestMatch**
4. Cochez le rôle **Admin**
5. Cliquez sur **"Save"**

### 8.3 Créer un Match Manager

Répétez les étapes 8.1-8.2 avec :
- **Username** : `mamadou.diallo`
- **First Name** : `Mamadou`
- **Last Name** : `Diallo`
- **Email** : `mamadou.diallo@gestmatch.sn`
- **Rôle** : **MatchManager**

### 8.4 Créer un utilisateur standard

- **Username** : `fatou.sow`
- **First Name** : `Fatou`
- **Last Name** : `Sow`
- **Email** : `fatou.sow@gestmatch.sn`
- **Rôle** : **User**

## Étape 9 : Redémarrer l'API GestMatch

```powershell
# Arrêter l'API
docker-compose stop api

# Redémarrer avec les nouvelles variables
docker-compose up -d api

# Vérifier les logs
docker logs gestmatch-api --tail 50
```

Vérifiez qu'il n'y a pas d'erreurs liées à Zitadel.

## Étape 10 : Tester l'authentification

### 10.1 Avec Swagger UI

1. Ouvrez **http://localhost:5000/swagger**
2. Cliquez sur le bouton **"Authorize"** 🔒 (en haut à droite)
3. Vous serez redirigé vers Zitadel
4. Connectez-vous avec un compte (ex: `mamadou.diallo@gestmatch.sn`)
5. Autorisez l'application
6. Vous serez redirigé vers Swagger avec un token JWT

### 10.2 Tester un endpoint protégé

1. Dans Swagger, testez `POST /api/matches`
2. Si vous êtes connecté en tant que **MatchManager** → ✅ Succès
3. Si vous êtes connecté en tant que **User** → ❌ 403 Forbidden

### 10.3 Avec curl/PowerShell (Token JWT)

```powershell
# Obtenir un token avec Client Credentials (pour l'API)
$clientId = "<API_CLIENT_ID>"
$clientSecret = "<API_CLIENT_SECRET>"
$credentials = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("${clientId}:${clientSecret}"))

$body = @{
    grant_type = "client_credentials"
    scope = "openid profile email"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "http://localhost:8080/oauth/v2/token" `
    -Method POST `
    -Headers @{
        Authorization = "Basic $credentials"
        "Content-Type" = "application/json"
    } `
    -Body $body

$token = $response.access_token

# Utiliser le token pour appeler l'API
$headers = @{
    Authorization = "Bearer $token"
}

Invoke-RestMethod -Uri "http://localhost:5000/api/matches" `
    -Method GET `
    -Headers $headers
```

### 10.4 Avec Resource Owner Password Flow (DEV uniquement)

```powershell
$clientId = "<API_CLIENT_ID>"
$clientSecret = "<API_CLIENT_SECRET>"
$credentials = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("${clientId}:${clientSecret}"))

$body = @{
    grant_type = "password"
    username = "mamadou.diallo@gestmatch.sn"
    password = "VotreMotDePasse"
    scope = "openid profile email urn:zitadel:iam:org:project:id:<PROJECT_ID>:aud"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "http://localhost:8080/oauth/v2/token" `
    -Method POST `
    -Headers @{
        Authorization = "Basic $credentials"
        "Content-Type" = "application/json"
    } `
    -Body $body

$token = $response.access_token
```

## Étape 11 : Configurer les claims personnalisés (optionnel)

Pour ajouter les rôles dans le token JWT :

### 11.1 Créer une Action

1. Dans Zitadel Console, allez dans **"Actions"**
2. Cliquez sur **"+ New"**
3. Configuration :
   - **Name** : `Add Roles to Token`
   - **Script** :

```javascript
function complementToken(ctx, api) {
  if (ctx.v1.user.grants) {
    const roles = ctx.v1.user.grants
      .filter(g => g.projectId === '<PROJECT_ID>')
      .flatMap(g => g.roles || []);
    
    if (roles.length > 0) {
      api.v1.setClaim('role', roles[0]);
      api.v1.setClaim('roles', roles);
    }
  }
}
```

Remplacez `<PROJECT_ID>` par votre ID de projet.

4. Cliquez sur **"Save"**

### 11.2 Activer l'Action

1. Allez dans l'onglet **"Flows"** → **"Complement Token**
2. Cliquez sur **"+ Trigger"**
3. Sélectionnez votre action **"Add Roles to Token"**
4. Cliquez sur **"Save"**

Désormais, les tokens JWT contiendront les claims `role` et `roles`.

## Commandes utiles

### Redémarrer Zitadel

```powershell
docker-compose restart zitadel
```

### Voir les logs Zitadel

```powershell
docker logs gestmatch-zitadel -f
```

### Réinitialiser Zitadel (ATTENTION : supprime toutes les données)

```powershell
# Arrêter et supprimer les containers
docker-compose down

# Supprimer le volume de données Zitadel
docker volume rm gestmatch_zitadel_data

# Redémarrer (réinitialisera Zitadel)
docker-compose up -d
```

### Backup de la base Zitadel

```powershell
# Exporter la base de données Zitadel
docker exec gestmatch-zitadel-db pg_dump -U zitadel zitadel > zitadel_backup.sql

# Restaurer
docker exec -i gestmatch-zitadel-db psql -U zitadel zitadel < zitadel_backup.sql
```

## Accès aux services

| Service | URL | Identifiants |
|---------|-----|--------------|
| **Zitadel Console** | http://localhost:8080 | admin@gestmatch.sn / Admin@2026! |
| **GestMatch API** | http://localhost:5000 | Token JWT |
| **Swagger UI** | http://localhost:5000/swagger | - |
| **pgAdmin** | http://localhost:5050 | admin@gestmatch.sn / admin123 |

## Ports utilisés

- **5000** : GestMatch API
- **5432** : PostgreSQL (GestMatch)
- **5050** : pgAdmin
- **8080** : Zitadel Console & API
- **5433** : PostgreSQL (Zitadel) - interne au réseau Docker

## Dépannage

### Zitadel ne démarre pas

Vérifiez les logs :
```powershell
docker logs gestmatch-zitadel --tail 100
```

Vérifiez que le mot de passe admin respecte les règles :
- Au moins 8 caractères
- 1 majuscule, 1 minuscule, 1 chiffre, 1 caractère spécial

### "Database not ready"

Attendez que PostgreSQL soit prêt :
```powershell
docker logs gestmatch-zitadel-db
```

### Token JWT invalide

Vérifiez que :
- `ZITADEL_AUTHORITY` dans `.env` est `http://localhost:8080` (sans `/` à la fin)
- `ZITADEL_AUDIENCE` correspond au Project ID
- Le Client ID et Secret sont corrects

Décodez le token sur https://jwt.io pour vérifier le contenu.

### Swagger ne redirige pas vers Zitadel

Vérifiez que :
- L'application Mobile a bien `http://localhost:5000/signin-oidc` dans les Redirect URIs
- `ZITADEL_CLIENT_ID` dans `.env` correspond au Client ID de l'app Mobile (pas l'API)

## Sécurité - Production

⚠️ **Ne PAS utiliser cette configuration en production !**

Pour la production :

1. **Activez HTTPS** :
   - Ajoutez un reverse proxy (Nginx, Traefik)
   - Configurez des certificats SSL/TLS
   - Changez `ZITADEL_EXTERNALSECURE=true`

2. **Changez tous les mots de passe** :
   - `ZITADEL_MASTERKEY` : Utilisez une clé aléatoire de 32+ caractères
   - `ZITADEL_DB_PASSWORD` : Mot de passe fort
   - `ZITADEL_ADMIN_PASSWORD` : Mot de passe fort
   - `POSTGRES_PASSWORD` : Mot de passe fort

3. **Utilisez des secrets** :
   - Docker Secrets
   - Azure Key Vault
   - HashiCorp Vault

4. **Limitez l'accès** :
   - Firewall sur les ports
   - Réseau privé pour la base de données
   - VPN/VPC pour Zitadel

5. **Activez les logs et monitoring** :
   - Centralisez les logs (ELK, Loki)
   - Monitoring (Prometheus, Grafana)
   - Alertes sur échecs d'authentification

## Ressources

- **Documentation Zitadel** : https://zitadel.com/docs
- **GitHub Zitadel** : https://github.com/zitadel/zitadel
- **Exemples Docker Compose** : https://github.com/zitadel/zitadel/tree/main/docs/docs/self-hosting/deploy
- **API Reference** : https://zitadel.com/docs/apis/introduction
