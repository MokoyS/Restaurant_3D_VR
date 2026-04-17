# Restaurant 3D VR — IIMresto

Projet scolaire Unity 6.4 (URP) — Simulation interactive d'un restaurant en 3D.  
**Rendu :** Vendredi 17 Avril 2026  
**Cible :** Build APK Android + Desktop (XR Device Simulator)

---

## Aperçu

Explorez un restaurant en 3D à la première personne. Interagissez avec les éléments du décor, gérez l'éclairage, animez la porte d'entrée, et aménagez librement la salle en plaçant des objets depuis l'inventaire.

---

## Décors & Environnement

- **Salle principale** — sol carrelé, plafond, 4 murs, encadrement de porte
- **Éclairage** — 6 suspensions au plafond (PendantLight + Shade + fil), 4 appliques murales (WallSconce)
- **6 tables rondes** — chacune avec nappe, 4 chaises, bougie, 2 verres
- **Bar** — comptoir, étagère, 7 bouteilles, 5 tabourets
- **4 plantes d'intérieur** — avec pot
- **Moulures décoratives** au plafond
- **Porte animée** avec encadrement (Frame_L/R/Top)
- **Jardin extérieur** et mur de pierre périmétrique (décoratif)
- **Post-processing** — Bloom + Ambient Occlusion sur tout l'environnement

---

## Interactions

### Porte (`E`)
- Approchez-vous de la poignée de porte
- Appuyez sur **E** pour ouvrir/fermer la porte (animation rotation 90°)
- Un **son de grincement** est joué à chaque actionnement

### Interrupteur lumière (`E`)
- Interrupteur positionné à gauche de l'entrée
- Appuyez sur **E** à portée pour allumer/éteindre les **16 lumières** de la salle
- Un **son de clic** est joué au toggle

### Inventaire (`I`)
Ouvrez l'inventaire pour placer des objets dans la salle :

| Touche | Objet |
|--------|-------|
| `I` | Ouvrir/fermer l'inventaire |
| Bouton **Table** | Table avec 4 pieds |
| Bouton **Chaise** | Chaise |
| Bouton **Assiette** | Assiette blanche |
| Bouton **Verre** | Verre transparent |
| Bouton **Reset tout** | Supprime tous les objets placés |

**Pendant le placement (ghost bleu semi-transparent) :**
- **Clic gauche** → pose l'objet
- **R** → rotation 90°
- **Molette** → rotation libre
- **Clic droit / Escape** → annuler

**Après placement :**
- **Clic + drag** → déplacer l'objet
- **R** → rotation 90°
- **Delete / Backspace** → supprimer l'objet

Un **son pop** est joué à chaque pose d'objet.

---

## Navigation

- **WASD / ZQSD / Flèches** — se déplacer
- **Clic droit maintenu** — regarder autour
- **Shift** — sprint

> Le joueur est doté d'un **CharacterController** (collisions physiques avec murs et mobilier).  
> En cas de chute hors de la scène, le joueur est automatiquement **respawné** à l'entrée du restaurant.

---

## Sons

| Événement | Fichier |
|-----------|---------|
| Ouverture/fermeture de porte | `Porte Sound Effects 411632.mp3` |
| Toggle interrupteur lumière | `Effet sonore interrupteur lumière.mp3` |
| Pose d'un objet (inventaire) | `Effets sonores pop dragon.mp3` |

---

## Scripts

| Script | Rôle |
|--------|------|
| `FirstPersonController.cs` | Déplacement FPS + collisions (CharacterController) + gravité |
| `CandleFlicker.cs` | Scintillement des bougies via Perlin noise |
| `CameraPath.cs` | Chemin cinématique de caméra |
| `DoorController.cs` | Animation porte 0→90° (Coroutine Lerp) + son |
| `LightSwitch.cs` | Toggle ON/OFF des lumières + son |
| `XRInteractor.cs` | Détection de proximité (E) + overlay d'action |
| `ObjectSpawner.cs` | Instanciation de prefabs |
| `SpawnUI.cs` | Interface touches 1/2/3/4 (raccourcis) |
| `InventoryUI.cs` | Panel inventaire (I) + sélection d'objet à placer |
| `PlacementManager.cs` | Ghost preview + placement au clic + rotation R + son |
| `ObjectManipulator.cs` | Drag XZ + rotation R 90° + suppression Delete |
| `DecoAnimator.cs` | Animation idle bobbing (décoration) |
| `FallDetector.cs` | Respawn joueur si chute sous la scène |

---

## Prefabs spawnables

| Prefab | Description |
|--------|-------------|
| `SpawnableTable` | Table ronde avec 4 pieds et plateau |
| `SpawnableChair` | Chaise avec siège et dossier |
| `SpawnableAssiette` | Assiette blanche plate |
| `SpawnableVerre` | Verre transparent |

---

## Structure du projet

```
Assets/
├── Materials/       — Matériaux URP (bois, verre, interrupteur…)
├── Prefabs/         — Objets spawnables
├── Scenes/
│   └── IIMresto.unity
├── Scripts/         — Tous les scripts C#
├── Sons/            — Fichiers audio .mp3
└── Textures/        — Textures procédurales (bois porte)
```

---

## Build Android

- **Platform :** Android
- **Minimum API Level :** 22+
- `OnMouseDown` / `OnMouseDrag` → mappés sur les événements touch Android
- Désactiver `XR Device Simulator` avant le build
