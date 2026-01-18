# 🤖 GestMatch AI Agents System

Ce dossier contient les **agents spécialisés** pour guider le développement du projet GestMatch avec GitHub Copilot.

## 📋 Agents Disponibles

### 🧠 [Backend Agent](./backend.agent.md)
**Mission** : Architecture .NET 8 Minimal API, EF Core, PostgreSQL
**Utiliser quand** : Création/modification d'endpoints, entités, services backend

### 🔐 [Security Agent](./security.agent.md)
**Mission** : Authentification Zitadel, JWT, autorisation par rôles
**Utiliser quand** : Configuration auth, sécurisation endpoints, gestion tokens

### 📱 [Mobile Agent](./mobile.agent.md)
**Mission** : Application .NET MAUI, MVVM, consommation API
**Utiliser quand** : Développement pages, ViewModels, services mobile

### 🧪 [QA Agent](./qa.agent.md)
**Mission** : Tests unitaires/intégration, qualité code, validations
**Utiliser quand** : Écriture tests, review code, validation migrations

### 🐳 [DevOps Agent](./devops.agent.md)
**Mission** : Docker, CI/CD, déploiement, monitoring
**Utiliser quand** : Configuration containers, pipelines, infrastructure

## 🚀 Comment Utiliser les Agents

### Méthode 1 : Dans GitHub Copilot Chat

1. Ouvrir **GitHub Copilot Chat** dans VS Code
2. Copier le contenu de l'agent souhaité (ex: `backend.agent.md`)
3. Coller dans le chat avec votre demande :

```
[Copier le contenu de backend.agent.md]

Maintenant, génère un endpoint pour créer un match avec validation des données.
```

### Méthode 2 : Référence Rapide

```
@workspace /new Utilise les règles du Backend Agent (.ai/backend.agent.md) 
pour créer le service de gestion des paiements
```

### Méthode 3 : Prompt Personnalisé

Créez vos propres prompts en combinant plusieurs agents :

```
Je veux créer une nouvelle fonctionnalité de notification.

Suis ces agents :
- Backend Agent pour l'API
- Security Agent pour l'autorisation
- Mobile Agent pour l'interface

Génère le code complet.
```

## ✅ Validation Automatique

Les agents sont couplés aux **GitHub Actions** qui valident automatiquement :

### CI Pipeline (`.github/workflows/ci.yml`)
- ✅ Build .NET
- ✅ Tests unitaires
- ✅ Code formatting
- ✅ EF Core migrations
- ✅ Docker build
- ✅ Security scan

### Code Review (`.github/workflows/code-review.yml`)
- ✅ Checklist automatique sur PR
- ✅ Validation des règles des agents
- ✅ Suggestions d'amélioration

## 🎯 Workflow Recommandé

1. **Choisir l'agent** approprié pour votre tâche
2. **Activer l'agent** dans Copilot Chat
3. **Générer le code** avec Copilot
4. **Créer une branche** : `git checkout -b feature/ma-fonctionnalité`
5. **Commit** : `git commit -m "feat: description"`
6. **Push** : `git push origin feature/ma-fonctionnalité`
7. **Ouvrir une PR** sur GitHub
8. **Validation automatique** via GitHub Actions
9. **Review** par GitHub Copilot Code Review
10. **Merge** après validation

## 📊 Règles de Qualité Enforced

- ✅ Code coverage > 80%
- ✅ Zéro warnings en build
- ✅ Code formatting conforme (`.editorconfig`)
- ✅ Pas de secrets hardcodés
- ✅ Migrations EF Core valides
- ✅ Docker build réussi
- ✅ Security scan sans vulnérabilités critiques

## 🔧 Configuration

### EditorConfig
Le fichier `.editorconfig` à la racine définit les règles de formatage.

### GitHub Actions
Les workflows dans `.github/workflows/` valident automatiquement le code.

### Environment Variables
Utilisez `.env.example` comme template et ne committez jamais `.env`.

## 📚 Exemples d'Utilisation

### Créer un nouvel endpoint

```
Utilise Backend Agent et Security Agent.

Crée un endpoint PUT /api/matches/{id}/cancel pour annuler un match.
Seuls Admin et MatchManager peuvent l'utiliser.
Ajoute la validation et les tests.
```

### Ajouter une page MAUI

```
Utilise Mobile Agent.

Crée une page de statistiques pour les gestionnaires de matchs.
Affiche le nombre de billets vendus par match.
Suis le pattern MVVM strict.
```

### Optimiser Docker

```
Utilise DevOps Agent.

Optimise le Dockerfile pour réduire la taille de l'image.
Ajoute un health check pour l'API.
```

## 🆘 Support

Si un agent génère du code qui ne respecte pas les règles :
1. La **CI pipeline** le détectera
2. La **PR sera bloquée**
3. Ajustez le prompt et régénérez

---

**💡 Astuce** : Combinez plusieurs agents pour des tâches complexes !

**🎯 Objectif** : Code production-ready avec validation automatique à chaque étape.
