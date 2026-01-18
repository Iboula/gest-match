# 🎯 Guide de Démarrage - GestMatch

## ✅ Ce qui a été créé

### 🏗️ Architecture Backend (.NET 8)

```
src/
├── GestMatch.Domain/           # Entités métier
│   ├── Entities/
│   │   ├── User.cs            # Utilisateur synchronisé avec Zitadel
│   │   ├── Match.cs           # Match sportif
│   │   ├── Ticket.cs          # Billet avec QR Code
│   │   └── Payment.cs         # Paiement (Wave, Orange Money, etc.)
│   └── Enums/                 # Énumérations (Rôles, Statuts, etc.)
│
├── GestMatch.Application/      # Services et DTOs
│   ├── DTOs/                  # Data Transfer Objects
│   └── Interfaces/            # Interfaces de services
│
├── GestMatch.Infrastructure/   # EF Core et implémentations
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/    # Configurations Fluent API
│   └── Services/              # Implémentation des services
│       ├── MatchService.cs
│       ├── TicketService.cs
│       ├── QrCodeService.cs
│       └── UserService.cs
│
└── GestMatch.Api/             # Minimal API
    ├── Program.cs
    ├── Endpoints/             # Endpoints REST
    │   ├── MatchEndpoints.cs
    │   ├── TicketEndpoints.cs
    │   └── UserEndpoints.cs
    └── Extensions/            # Extensions de configuration
```

### 📱 Frontend Mobile (.NET MAUI)

```
src/GestMatch.MauiApp/
├── Models/                    # Modèles de données
├── Services/                  # Services API et Auth
│   ├── AuthService.cs        # Authentification Zitadel
│   ├── ApiService.cs         # Communication HTTP
│   ├── MatchService.cs
│   ├── TicketService.cs
│   └── NavigationService.cs
├── ViewModels/               # ViewModels MVVM
│   ├── LoginViewModel.cs
│   ├── MatchListViewModel.cs
│   ├── MatchDetailViewModel.cs
│   ├── TicketPurchaseViewModel.cs
│   └── MyTicketsViewModel.cs
└── Views/                    # Pages XAML
    ├── LoginPage.xaml
    ├── MatchListPage.xaml
    ├── MatchDetailPage.xaml
    ├── TicketPurchasePage.xaml
    └── MyTicketsPage.xaml
```

---

## 🚀 Étapes de Démarrage

### 1️⃣ Prérequis

- ✅ .NET 8 SDK installé
- ✅ Docker Desktop installé
- ✅ Visual Studio 2022 (ou Rider/VS Code)
- ✅ Compte Zitadel configuré

### 2️⃣ Configuration Zitadel

1. **Créer un projet dans Zitadel**
2. **Créer une application OAuth/OIDC**
3. **Configurer les rôles** :
   ```
   - Admin
   - MatchManager
   - User
   ```
4. **Copier les valeurs** :
   - Authority URL
   - Client ID
   - Client Secret

### 3️⃣ Configuration de l'environnement

1. **Copier le fichier d'environnement**
   ```bash
   cp .env.example .env
   ```

2. **Éditer `.env`** avec vos valeurs Zitadel :
   ```env
   ZITADEL_AUTHORITY=https://your-instance.zitadel.cloud
   ZITADEL_CLIENT_ID=your_client_id
   ZITADEL_CLIENT_SECRET=your_client_secret
   ZITADEL_AUDIENCE=your_client_id
   ```

### 4️⃣ Démarrer le Backend

**Option A : Avec Docker (Recommandé)**

```bash
# Démarrer PostgreSQL + API + pgAdmin
docker-compose up -d

# Voir les logs
docker-compose logs -f api

# L'API sera disponible sur http://localhost:5000
# Swagger UI : http://localhost:5000/swagger
```

**Option B : Manuellement (pour développement)**

```bash
# Restaurer les packages
dotnet restore

# Créer la migration initiale
cd src/GestMatch.Api
dotnet ef migrations add InitialCreate --project ../GestMatch.Infrastructure

# Appliquer les migrations
dotnet ef database update --project ../GestMatch.Infrastructure

# Démarrer l'API
dotnet run
```

### 5️⃣ Tester l'API

```bash
# Health Check
curl http://localhost:5000/health

# Liste des matchs (public)
curl http://localhost:5000/api/matches

# Swagger UI
# Ouvrir http://localhost:5000/swagger dans le navigateur
```

### 6️⃣ Configurer l'application Mobile

1. **Ouvrir `src/GestMatch.MauiApp/MauiProgram.cs`**
2. **Modifier l'URL de l'API** :
   ```csharp
   services.AddHttpClient("GestMatchApi", client =>
   {
       client.BaseAddress = new Uri("http://YOUR_IP:5000"); // Remplacer YOUR_IP
   });
   ```

3. **Pour Android** : Utiliser l'IP de votre machine (pas localhost)
4. **Pour iOS Simulator** : Utiliser `http://localhost:5000`

### 7️⃣ Lancer l'application Mobile

```bash
cd src/GestMatch.MauiApp

# Pour Android
dotnet build -t:Run -f net8.0-android

# Pour iOS
dotnet build -t:Run -f net8.0-ios
```

---

## 🔐 Authentification et Autorisation

### Endpoints Protégés

| Endpoint | Rôles Autorisés | Description |
|----------|----------------|-------------|
| `POST /api/matches` | Admin, MatchManager | Créer un match |
| `PUT /api/matches/{id}` | Admin, MatchManager | Modifier un match |
| `DELETE /api/matches/{id}` | Admin, MatchManager | Supprimer un match |
| `POST /api/tickets/purchase` | User, MatchManager, Admin | Acheter un billet |
| `POST /api/tickets/scan` | MatchManager, Admin | Scanner un billet |

### Utiliser Swagger avec JWT

1. Obtenir un token JWT depuis Zitadel
2. Dans Swagger UI, cliquer sur **"Authorize"**
3. Entrer le token : `Bearer YOUR_TOKEN`
4. Tester les endpoints protégés

---

## 📊 Base de Données

### Structure des Tables

- **Users** : Utilisateurs synchronisés avec Zitadel
- **Matches** : Matchs sportifs
- **Tickets** : Billets avec QR Code
- **Payments** : Paiements mobiles

### Accéder à pgAdmin

```
URL : http://localhost:5050
Email : admin@gestmatch.sn
Password : (voir .env)

Serveur PostgreSQL :
- Host : postgres
- Port : 5432
- Database : gestmatch
- Username : gestmatch
```

---

## 🎫 Fonctionnalités Implémentées

### ✅ Backend API

- [x] Authentification JWT avec Zitadel
- [x] Gestion des matchs (CRUD)
- [x] Billetterie avec QR Code
- [x] Paiements mobiles (Wave, Orange Money, Free Money)
- [x] Scan de billets
- [x] Autorisation par rôles
- [x] Swagger UI sécurisé
- [x] Docker & Docker Compose

### ✅ Frontend Mobile

- [x] Pages de connexion
- [x] Liste des matchs
- [x] Détails d'un match
- [x] Achat de billet
- [x] Mes billets avec QR Code
- [x] Architecture MVVM
- [x] Navigation

---

## 🛠️ Prochaines Étapes

### Phase 1 - MVP (À compléter)

1. **Authentification Zitadel complète**
   - Implémenter OIDC flow dans MAUI
   - Gérer le refresh token
   - Stockage sécurisé des tokens

2. **Paiements mobiles réels**
   - Intégration Wave API
   - Intégration Orange Money API
   - Webhooks de confirmation

3. **Scanner QR Code**
   - Implémenter le scanner dans MAUI
   - Mode hors-ligne pour le scan

4. **Tests et déploiement**
   - Tests unitaires
   - Tests d'intégration
   - Déploiement sur Azure/AWS

### Phase 2 - Évolutions

- [ ] Notifications push
- [ ] Partage de billets via WhatsApp
- [ ] Stats en temps réel
- [ ] Live score
- [ ] Gestion des équipes
- [ ] Sponsoring

---

## 📝 Notes Importantes

### Sécurité

- ⚠️ **Ne jamais commiter le fichier `.env`**
- ⚠️ Changer les mots de passe par défaut en production
- ⚠️ Utiliser HTTPS en production
- ⚠️ Le QR Code secret doit être changé en production

### Développement

- Les migrations sont appliquées automatiquement en mode Development
- En production, appliquer les migrations manuellement
- pgAdmin est optionnel (à désactiver en production)

### Configuration Mobile

- Pour tester sur un appareil physique, utiliser l'IP de votre machine
- Pour Android : Vérifier que le port 5000 est accessible
- Pour iOS : Configurer App Transport Security si HTTP

---

## 🆘 Support

Pour toute question :
- Documentation API : http://localhost:5000/swagger
- Logs Docker : `docker-compose logs -f`
- Vérifier le fichier [DOCKER.md](DOCKER.md) pour plus de détails

---

## 📄 Licence

© 2026 GestMatch - Tous droits réservés
