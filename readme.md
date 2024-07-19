# PEzBus

[![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)  [![forthebadge](http://forthebadge.com/images/badges/powered-by-electricity.svg)](http://forthebadge.com)

Certaines applications traitant de grands flux d'entrées/sorties de données (i.e les OMS, serveurs de jeux videos ...) nécessitent une grande organisation pour orchestrer l'impacte d'une nouvelle donnée ou action sur les données existantes. Dans ce cas précis, développer un système drivé par les évènements permet de créer un effets dominos et mettre un nom sur ces changements.

Le java a son package Guava de google, qui offre un bus d'évènement et implémente le pattern Publish / Subscribe et le rend très accessible grâce aux décorateurs de méthodes :

```java {"id":"01J2KXAXNVT7KR99R7Z2Q6EGFE"}
...
MyEvent event = new MyEvent();
eventBus.post(event);
@Subscribe
public void handleEvent(MyEvent event){
    ...
}



```

Ce mode de fonctionnement rend l'utilisation des évènements plus convenable et de surcroit plus simple.
L'idée de ce package et d'apporter ce mode de fonctionnement plus adéquat aux framework .NET

Par expérience, ce qui manque à cette fonctionalité est la capabilité de modéliser l'impacte d'un évènement sur le système. Pouvoir générer une documentation qui se base sur les évènements et les commentaires XML pour modéliser un système est à mon sens largement suffisant pour décrire le fonctionnement d'un système basé sur les évènements.

### Performance

Comme les performances sont fondamentalement importantes, voici des tests realisés :

Le test est lancé avec 100 instances implémentants IEventHandler. Le test publie N evénements qui provoqueront l'invocation des méthodes correspondantes et présentes dans les instances IEventHandler enregistrées. 

| Method        | N      | Mean          | Error          | StdDev        | Gen0     | Gen1     | Allocated  |
|-------------- |------- |--------------:|---------------:|--------------:|---------:|---------:|-----------:|
| PublishEvents | 1      |      1.132 us |      0.3809 us |     0.0209 us |   0.2098 |   0.0381 |    1.73 KB |
| PublishEvents | 1000   |    270.250 us |    200.5838 us |    10.9947 us |   4.3945 |   3.9063 |  292.09 KB |
| PublishEvents | 4000   |    640.621 us |    353.2004 us |    19.3601 us |  15.6250 |  14.6484 | 1155.36 KB |
| PublishEvents | 12000  |  1,790.901 us |  5,773.6316 us |   316.4722 us |  46.8750 |  42.9688 |  383.04 KB |
| PublishEvents | 25000  |  3,638.240 us |  9,831.3922 us |   538.8918 us |  97.6563 |  93.7500 |  789.14 KB |
| PublishEvents | 60000  |  8,941.344 us | 47,093.2707 us | 2,581.3411 us | 234.3750 | 226.5625 | 1883.47 KB |
| PublishEvents | 120000 | 16,967.658 us | 90,070.0816 us | 4,937.0451 us | 437.5000 | 406.2500 | 3755.42 KB |

### Pré-requis

Principalement du café et optionellement un clavier

### Installation

```sh {"id":"01J2KXAXNW7XF4PPWYZPVX70QG"}

 dotnet add package PEzBus



```

## Démarrage

L'utilisation est plutôt simple :

- Créer des classes représentants des évènements (accessoirement avec des attributs pour la transmission de donnée) qui implémente l'interface IEzEvent. L'idéale est de choisir des noms claires qui définissent l'évènement en question : i.e OrderCreatedEvent
- Implémenter des méthodes décorées avec l'attribut Subscribe comme suit :

```cs {"id":"01J2KXAXNW7XF4PPWYZQS3BWBZ"}
public class LaClass {
    [Subscribe(typeof(MonSuperEvent))]
    public void MaSuperMethode(){
        ...
    }
}


```

ou encore :

```cs {"id":"01J2KXAXNW7XF4PPWYZSSDTMHJ"}
public class LaClass {
   [Subscribe(typeof(MonSuperEvent))]
   public void MaSuperMethode(MonSuperEvent event){
       var eventArg = event.GetEventArg();
       eventArg.TransformationSayen();
   }
}


```

- Créer une instance du bus d'évènement :

```sh {"id":"01J2KXAXNW7XF4PPWYZTXSXBPX"}
EventBus eventBus = new();
```

- Inscrire une instance de la class dans ce bus d'évènement :

```ini {"id":"01J2KXAXNW7XF4PPWYZXPVRG3P"}
var maClass = new LaClass();
eventBus.Register<LaClass>(maClass);


```

- Publier l'évènement qui se produit :

```sh {"id":"01J2KXAXNW7XF4PPWYZXSY1Q4T"}
var event = new MonSuperEvent(...params);
eventBus.Publish(event);


```

Ensuite,les méthodes ayant comme paramètre de l'attribut Subscribe le type MonSuperEvent `[Subscribe(typeof(MonSuperEvent))]` seront invokés, si et seulement si une instance de classe contenant la méthode est inscrite dans le bus d'évènement.

## Versions

**Première version :** 1.0.1
Implémentation du bus d'évènements qui constitue le coeur du projet

**Actuel** : 1.4
+   Génération d'un schéma représentant les evènements et les méthodes qui les gérent
+   Refactoring du code pour ajouter encore plus de fonctionalités.

## TODO :

-   Ajouter la possibilité d'émettre de publier les évènements à un ou plusieurs serveurs distants.

## Auteurs

* __Aymane EL JAHRANI__ _alias_ [@eljaymane](https://github.com/eljaymane)

## License


