## 🐳 Démarrage avec Docker

### Prérequis
- Docker Desktop installé
- Docker Compose installé

### Configuration

1. **Copier le fichier d'environnement**
   ```bash
   cp .env.example .env
   ```

2. **Modifier les variables d'environnement dans `.env`**
   - Remplacer les valeurs Zitadel par vos vraies configurations
   - Changer les mots de passe par défaut

3. **Démarrer tous les services**
   ```bash
   docker-compose up -d
   ```

### Services disponibles

| Service | URL | Description |
|---------|-----|-------------|
| API | http://localhost:5000 | API REST GestMatch |
| Swagger UI | http://localhost:5000/swagger | Documentation interactive |
| PostgreSQL | localhost:5432 | Base de données |
| pgAdmin | http://localhost:5050 | Interface de gestion PostgreSQL |

### Commandes utiles

```bash
# Démarrer les services
docker-compose up -d

# Voir les logs
docker-compose logs -f api

# Arrêter les services
docker-compose down

# Arrêter et supprimer les volumes (⚠️ perte de données)
docker-compose down -v

# Rebuild l'API après modification du code
docker-compose up -d --build api

# Accéder au shell du conteneur API
docker exec -it gestmatch-api /bin/bash

# Accéder au shell PostgreSQL
docker exec -it gestmatch-postgres psql -U gestmatch -d gestmatch
```

### Migration de la base de données

Les migrations sont appliquées automatiquement au démarrage de l'API en mode Development.

Pour créer une nouvelle migration localement :

```bash
cd src/GestMatch.Api
dotnet ef migrations add NomDeLaMigration --project ../GestMatch.Infrastructure
```

### Connexion à pgAdmin

1. Ouvrir http://localhost:5050
2. Se connecter avec :
   - Email : `admin@gestmatch.sn` (ou valeur dans .env)
   - Password : `admin_secure_password` (ou valeur dans .env)
3. Ajouter un nouveau serveur :
   - Host : `postgres`
   - Port : `5432`
   - Database : `gestmatch`
   - Username : `gestmatch`
   - Password : valeur de `POSTGRES_PASSWORD` dans .env

### Configuration Zitadel

Pour configurer l'authentification Zitadel :

1. **Créer un projet dans Zitadel**
2. **Créer une application OAuth/OIDC**
3. **Configurer les rôles** :
   - `Admin`
   - `MatchManager`
   - `User`
4. **Copier les valeurs dans `.env`** :
   - `ZITADEL_AUTHORITY`
   - `ZITADEL_CLIENT_ID`
   - `ZITADEL_CLIENT_SECRET`

### Tester l'API

#### Sans authentification (endpoints publics)
```bash
# Health check
curl http://localhost:5000/health

# Liste des matchs
curl http://localhost:5000/api/matches
```

#### Avec authentification (nécessite un token JWT)
```bash
# Remplacer YOUR_JWT_TOKEN par un token Zitadel valide
curl -H "Authorization: Bearer YOUR_JWT_TOKEN" \
     http://localhost:5000/api/users/me
```

### Troubleshooting

#### L'API ne démarre pas
```bash
# Vérifier les logs
docker-compose logs api

# Vérifier que PostgreSQL est démarré
docker-compose ps postgres
```

#### Erreur de connexion à PostgreSQL
```bash
# Redémarrer PostgreSQL
docker-compose restart postgres

# Vérifier les variables d'environnement
docker-compose config
```

#### Réinitialiser complètement le projet
```bash
docker-compose down -v
docker-compose up -d --build
```
