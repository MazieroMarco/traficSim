# traficSim - Le simulateur de trafic autoroutier

Ce projet consiste à créer une simulateur de tronçon d'autoroute en tenps réel afin de pouvoir en calculer le débit de voitures en fonction de toute une liste de paramètres. Le trafic de voitures évoluera en temps réel en fonction de toute une liste de paramètres appliqués. L'affichage graphique du tronçon d'autoroute sera fait via le logiciel Unity 3D

# Build - Version 1.1

La version 1.1 contient un ajout majeur qui est la fonctionnalité qui permet à l'utilisateur de bloquer une route au choix. Divers autres changements ont été faits.

- Ajout de fonctionnalité de blocage de route
- Ajustement des polices dépendant la résolution (meilleure gestion des résolutions)
- Suppression de l'affichage des IPS (Images Par Seconde)

# Build - Version 1.0

La version 1.0 comptent toutes les fonctionnalités prévues à la base plus quelques options supplémentaires :
- Le nombre de voies praticables
- La taille du tronçon d'autoroute
- La limitation de vitesse
- La densité du trafic
- La fréquence des pannes
- Changement de la météo
- Densité de camions
- Limitation de vitesse des camions
- Système jour / nuit
- Changement de l'heure du jour
- Changement de location (forêt / désert)
- Option de vitesse de défilement
- Nouvel écran titre
- Nouvel écran de paramètres
- Split des débits par directions
- Calcul des débits maximums et minimums sur 30 sec, 5 min et 30 min
- Gestion des accidents
- Nouvelle interface

Le comportement des voitures à été revu et de gros bugs ont été résolus. Le dépassement ne cause plus de problèmes et les distances de sécurité sont respectées selon la vitesse et la météo.

# Build - Version 0.9

La version 0.9 ne comprend pas encore toutes les fonctionnalités prévues dans la version finale mais propose un système de simulation de trafic sur un tronçon avec quelques option modifibles :

  - Le nombre de voies praticables
  - La taille du tronçon d'autoroute
  - La limitation de vitesse
  - La densité du trafic
  
Les voitures possèdent quelques fonctionnalités pour rendre la simulation cohérente

  - La génération automatique des voitures sur le tronçon
  - La gestion des détections des autres voitures. Les voitures freinent pour éviter la collision
  - La gestion du dépassement. Les voitures cherchent à dépasser si la voiture de devant est lente
  
Des améliorations et ajouts de fonctionnalités sont prévus dans les futures versions du simulateur.

  - Gestion des collisions et accidents
  - Plus de paramètres modifiables
  - Présence de véhicules à limitation de vitesse (camions, etc...)
  - Options pour fermer une des voies de circulation
  - Gestion des effets météo
  
# Installation du simulateur

Le simulateur est disponible pour les systèmes Windows x64 et MacOs x64

1. Téléchargez la version du simulateur compatible avec votre système dans "Builds/"
2. Cliquez sur l'application
3. Amusez vous !
