# 🏟️ GestMatch - Gestion de Matchs et Billetterie

Application mobile de gestion de matchs sportifs avec billetterie numérique adaptée au contexte sénégalais.

## 🎯 Phase 1 - MVP

- ✅ Gestion des matchs par gestionnaires
- ✅ Consultation publique des matchs
- ✅ Billetterie numérique avec QR Code
- ✅ Paiement mobile (Wave, Orange Money, Free Money)
- ✅ Contrôle d'accès par rôles (Admin, MatchManager, User)

## 🛠️ Stack Technique

### Backend
- **.NET 8** - Minimal API
- **Entity Framework Core** - ORM
- **PostgreSQL** - Base de données
- **Zitadel** - Authentification OIDC/OAuth2
- **Docker** - Conteneurisation

### Frontend
- **.NET MAUI** - Application mobile cross-platform
- **MVVM Pattern**

## 📁 Structure du Projet

```
GestMatch/
├── src/
│   ├── GestMatch.Domain/           # Entités métier
│   ├── GestMatch.Application/      # Services, DTOs, Interfaces
│   ├── GestMatch.Infrastructure/   # EF Core, Repositories
│   ├── GestMatch.Api/             # Minimal API, Endpoints
│   └── GestMatch.MauiApp/         # Application mobile
├── docker-compose.yml
└── README.md
```

## 🚀 Démarrage Rapide

### Prérequis
- .NET 8 SDK
- Docker & Docker Compose
- Visual Studio 2022 ou VS Code

### Lancement avec Docker

```bash
# Démarrer tous les services
docker-compose up -d

# L'API sera disponible sur http://localhost:5000
# Swagger UI: http://localhost:5000/swagger
```

### Variables d'environnement

Créer un fichier `.env` :

```env
POSTGRES_USER=gestmatch
POSTGRES_PASSWORD=your_password
POSTGRES_DB=gestmatch
ZITADEL_AUTHORITY=https://your-zitadel-instance.com
ZITADEL_CLIENT_ID=your_client_id
ZITADEL_CLIENT_SECRET=your_client_secret
```

## 👥 Rôles Utilisateurs

### 🔴 Admin (Super Admin)
- Gestion complète du système
- Validation des gestionnaires
- Accès back-office

### 🟡 MatchManager (Gestionnaire)
- Créer/modifier/annuler des matchs
- Définir les billets (prix, quantité)
- Consulter les ventes

### 🟢 User (Public)
- Consulter les matchs
- Acheter des billets
- Recevoir des billets avec QR Code

## 🎫 Fonctionnalités Billetterie

- **Types de billets** : Standard, VIP, Gratuit
- **Paiement mobile** : Wave, Orange Money, Free Money
- **Billet numérique** : QR Code unique
- **Contrôle d'accès** : Scan QR Code à l'entrée

## 📱 Écrans Application Mobile

### Gestionnaire
1. Dashboard
2. Créer un match
3. Détails match
4. Gestion billetterie
5. Ventes

### Utilisateur
1. Liste des matchs
2. Recherche/Filtres
3. Détails match
4. Achat billet
5. Mes billets (QR Code)

## 🔐 Sécurité

- Authentification via **Zitadel (OIDC)**
- JWT Tokens
- Authorization Policies par rôle
- HTTPS obligatoire en production

## 📄 License

Propriétaire - © 2026 GestMatch
