Imports System
Imports System.Linq
Namespace Common.DataModel
    ''' <summary>
    ''' Represents the state of the entity relative to the unit of work.
    ''' </summary>
    Public Enum EntityState
        ''' <summary>
        ''' The object exists but is not being tracked. 
        ''' An entity is in this state immediately after it has been created and before it is added to the unit of work. 
        ''' An entity is also in this state after it has been removed from the unit of work by calling the IUnitOfWork.Detach method.
        ''' </summary>
        Detached = 1
        ''' <summary>
        ''' The object has not been modified since it was attached to the unit of work or since the last time that the IUnitOfWork.SaveChanges method was called.
        ''' </summary>
        Unchanged = 2
        ''' <summary>
        ''' The object is new, has been added to the unit of work, and the IUnitOfWork.SaveChanges method has not been called. 
        ''' After the changes are saved, the object state changes to Unchanged.
        ''' </summary>
        Added = 4
        ''' <summary>
        ''' The object has been deleted from the unit of work. After the changes are saved, the object state changes to Detached.
        ''' </summary>
        Deleted = 8
        ''' <summary>
        ''' One of the scalar properties on the object has been modified and the IUnitOfWork.SaveChanges method has not been called. 
        ''' After the changes are saved, the object state changes to Unchanged.
        ''' </summary>
        Modified = 16
    End Enum
    ''' <summary>
    ''' Represents the type of entity state change notification when the IUnitOfWork.SaveChanges method has been called.
    ''' </summary>
    Public Enum EntityMessageType
        ''' <summary>
        ''' The new entity has been added to the unit of work. 
        ''' </summary>
        Added
        ''' <summary>
        ''' The object has been deleted from the unit of work.
        ''' </summary>
        Deleted
        ''' <summary>
        ''' One of the properties on the object has been modified. 
        ''' </summary>
        Changed
    End Enum
    ''' <summary>
    ''' Provides information about entity state change notification when entity has been added, deleted or modified and the IUnitOfWork.SaveChanges method has been called.
    ''' </summary>
    ''' <typeparam name="TEntity">Added, deleted or modified entity type.</typeparam>
    Public Class EntityMessage(Of TEntity)
        Private _MessageType As EntityMessageType
        Private _Entity As TEntity
        ''' <summary>
        ''' The entity that has been added, deleted or modified.
        ''' </summary>
        Public ReadOnly Property Entity As TEntity
            Get
                Return _Entity
            End Get
        End Property
        ''' <summary>
        ''' The entity state change notification type.
        ''' </summary>
        Public ReadOnly Property MessageType As EntityMessageType
            Get
                Return _MessageType
            End Get
        End Property
        ''' <summary>
        ''' Initializes a new instance of the EntityMessage class.
        ''' </summary>
        ''' <param name="entity">An entity that has been added, deleted or modified.</param>
        ''' <param name="messageType">An entity state change notification type.</param>
        Public Sub New(ByVal entity As TEntity, ByVal messageType As EntityMessageType)
            Me._Entity = entity
            Me._MessageType = messageType
        End Sub
    End Class
End Namespace
