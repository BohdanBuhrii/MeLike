# MeLike
Simple social network using Blazor and MongoDB

### Structure:
- [MeLike.Data](https://github.com/BohdanBuhrii/MeLike/tree/master/MeLike.Data) - contains Mongo client configuration, context with collections, entities
- [MeLike.Data.Graph](https://github.com/BohdanBuhrii/MeLike/tree/master/MeLike.Data.Graph) - contains repositories for working with Neo4j graph database to represent connections betweet users
- [MeLike.Services](https://github.com/BohdanBuhrii/MeLike/tree/master/MeLike.Services) - project with all business logic
- [MeLike.App](https://github.com/BohdanBuhrii/MeLike/tree/master/MeLike.App) - frontend using [Blazor Server](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- [MeLikeDBUpdateWorker](https://github.com/BohdanBuhrii/MeLike/tree/master/MeLikeDBUpdateWorker) - a long running service to perform updates from change log

### Database collection examples:
- [Posts](https://github.com/BohdanBuhrii/MeLike/blob/master/MeLike.Data/CollectionExamples/posts.json)
- [Users](https://github.com/BohdanBuhrii/MeLike/blob/master/MeLike.Data/CollectionExamples/users.json)
- [User name change log](https://github.com/BohdanBuhrii/MeLike/blob/master/MeLike.Data/CollectionExamples/usernamechangelog.json)
