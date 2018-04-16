Imports System
Imports System.Linq
Imports System.Collections.ObjectModel
Namespace Common.DataModel
    ''' <summary>
    ''' The IReadOnlyRepository interface represents the read-only implementation of the Repository pattern 
    ''' such that it can be used to query entities of a given type. 
    ''' </summary>
    ''' <typeparam name="TEntity">Repository entity type.</typeparam>
    Public Interface IReadOnlyRepository(Of TEntity As Class)
        ''' <summary>
        ''' Returns an IQueryable object that can be used to query entities of a given type. 
        ''' </summary>
        Function GetEntities() As IQueryable(Of TEntity)
        ''' <summary>
        ''' The owner unit of work.
        ''' </summary>
        ReadOnly Property UnitOfWork As IUnitOfWork
        ''' <summary>
        ''' Gets an ObservableCollection that represents a local view of all Added, Unchanged, and Modified entities in this repository. 
        ''' This local view will stay in sync as entities are added or removed from the unit of work. 
        ''' Likewise, entities that are added to or removed from the local view will automatically be added to or removed from the unit of work.
        ''' </summary>
        ReadOnly Property Local As ObservableCollection(Of TEntity)
    End Interface
End Namespace
