## Helpers de Teste - Exemplo de Uso

Os helpers `TaskDtoFactory` e `TaskItemFactory` foram criados no padrão mostrado para gerar objetos mockados usando Bogus.

### TaskDtoFactory (Application.Tests)

Localizado em: `TaskManager.Application.Tests/TestData/TaskDtoFactory.cs`

**Opções de uso:**

```csharp
// Gerar um único DTO
var taskDto = TaskDtoFactory.GetMockedObject();

// Gerar uma lista com quantidade específica
var taskDtos = TaskDtoFactory.GetMockedList(quantity: 5);

// Usar métodos anteriores
var createDto = TaskDtoFactory.CreateCreateTaskDto();
var updateDto = TaskDtoFactory.CreateUpdateTaskDto(id: 123);
```

### TaskItemFactory (Domain.Tests)

Localizado em: `TaskManager.Domain.Tests/TestData/TaskItemFactory.cs`

**Opções de uso:**

```csharp
// Gerar uma única entidade
var taskItem = TaskItemFactory.GetMockedObject();

// Gerar uma lista com quantidade específica
var taskItems = TaskItemFactory.GetMockedList(quantity: 10);
```

### Exemplo em Teste Unitário

```csharp
[Fact]
public void SomeTest()
{
    // Gerar dados mockados
    var taskItem = TaskItemFactory.GetMockedObject();
    var taskDtos = TaskDtoFactory.GetMockedList(3);
    
    // Usar nos testes...
    Assert.NotNull(taskItem);
    Assert.Equal(3, taskDtos.Count);
}
```

### Dependências Instaladas

- **Bogus** v35.6.1 - Biblioteca para geração de dados fake
- Já estava instalado em `Application.Tests`
- Adicionado também em `Domain.Tests`

