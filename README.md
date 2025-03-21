
Go to the root of FeatGen folder that contains FeatGen.Server and FeatGen.Client folders.

```shell
cd FeatGen
dotnet tool restore
```

Run command below in terminal to generate initial migration script for PG.

```shell
dotnet ef dbcontext script --project FeatGen.Server --startup-project FeatGen.Server --context FeatGenDbContext --verbose -o migrations/initial_postgresql.sql
```


Error when execute the command above:

```shell
Unable to create a 'DbContext' of type 'FeatGenDbContext'. The exception 'No suitable constructor was found for entity type 'Vector'. The following constructors had parameters that could not be bound to properties of the entity type:
    Cannot bind 'v' in 'Vector(ReadOnlyMemory<float> v)'
    Cannot bind 's' in 'Vector(string s)'
Note that only mapped properties can be bound to constructor parameters. Navigations to related entities, including references to owned types, cannot be bound.' was thrown while attempting to create an instance. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
```

I think it's because 