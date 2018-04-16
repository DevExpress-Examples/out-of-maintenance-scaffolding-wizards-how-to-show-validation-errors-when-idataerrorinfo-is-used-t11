Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.Generic
Imports System.Linq
Imports System.Linq.Expressions
Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.POCO
Imports DevExpress.Mvvm.DataAnnotations
Imports Scaffolding.ValidationErrors.Common.Utils
Imports Scaffolding.ValidationErrors.Common.DataModel
Imports MessageBoxButton = System.Windows.MessageBoxButton
Imports MessageBoxImage = System.Windows.MessageBoxImage
Imports MessageBoxResult = System.Windows.MessageBoxResult

Namespace Scaffolding.ValidationErrors.Common.ViewModel
	Public MustInherit Partial Class SingleObjectViewModel(Of TEntity As Class, TPrimaryKey, TUnitOfWork As IUnitOfWork)
		Inherits SingleObjectViewModelBase(Of TEntity, TPrimaryKey, TUnitOfWork)
		Protected Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IRepository(Of TEntity, TPrimaryKey)))
			MyBase.New(unitOfWorkFactory, getRepositoryFunc)
		End Sub
	End Class
	<POCOViewModel> _
	Public MustInherit Class SingleObjectViewModelBase(Of TEntity As Class, TPrimaryKey, TUnitOfWork As IUnitOfWork)
		Implements ISingleObjectViewModel(Of TEntity, TPrimaryKey), ISupportParameter, IDocumentViewModel
		Private title_Renamed As Object
		Private ReadOnly getRepositoryFunc As Func(Of TUnitOfWork, IRepository(Of TEntity, TPrimaryKey))

		Protected Sub New(ByVal unitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork), ByVal getRepositoryFunc As Func(Of TUnitOfWork, IRepository(Of TEntity, TPrimaryKey)))
			UnitOfWorkFactory = unitOfWorkFactory
			Me.getRepositoryFunc = getRepositoryFunc
			UnitOfWork = UnitOfWorkFactory.CreateUnitOfWork()
			If Me.IsInDesignMode() Then
				Me.Entity = Me.Repository.Local.FirstOrDefault()
			Else
				OnInitializeInRuntime()
			End If
		End Sub
		Protected Overridable Sub OnInitializeInRuntime()
            Messenger.Default.Register(Of EntityMessage(Of TEntity))(Me, Sub(x) OnEntityMessage(x))
		End Sub

		Protected Overridable Sub OnEntityMessage(ByVal x As EntityMessage(Of TEntity))
			If Entity Is Nothing Then
				Return
			End If
			If x.MessageType = EntityMessageType.Deleted AndAlso Object.Equals(GetPrimaryKey(x.Entity), GetPrimaryKey(Entity)) Then
				Close()
			End If
		End Sub
		Private privateUnitOfWorkFactory As IUnitOfWorkFactory(Of TUnitOfWork)
		Protected Property UnitOfWorkFactory() As IUnitOfWorkFactory(Of TUnitOfWork)
			Get
				Return privateUnitOfWorkFactory
			End Get
			Private Set(ByVal value As IUnitOfWorkFactory(Of TUnitOfWork))
				privateUnitOfWorkFactory = value
			End Set
		End Property
		Private privateUnitOfWork As TUnitOfWork
		Protected Property UnitOfWork() As TUnitOfWork
			Get
				Return privateUnitOfWork
			End Get
			Private Set(ByVal value As TUnitOfWork)
				privateUnitOfWork = value
			End Set
		End Property
		Public ReadOnly Property Title() As Object
			Get
				Return title_Renamed
			End Get
		End Property
		Private privateEntity As TEntity
		Public Overridable Property Entity() As TEntity
			Get
				Return privateEntity
			End Get
			Set(ByVal value As TEntity)
				privateEntity = value
			End Set
		End Property
		Protected Overridable Sub OnEntityChanged()
			Update()
			If Me.Entity IsNot Nothing AndAlso Repository.HasPrimaryKey(Entity) Then
				RefreshLookUpCollections(GetPrimaryKey(Me.Entity))
			End If
		End Sub
		Protected Overridable Sub RefreshLookUpCollections(ByVal key As TPrimaryKey)
		End Sub
		Protected ReadOnly Property Repository() As IRepository(Of TEntity, TPrimaryKey)
			Get
				Return getRepositoryFunc(UnitOfWork)
			End Get
		End Property

		<Required> _
		Protected Overridable ReadOnly Property MessageBoxService() As IMessageBoxService
			Get
				Return Nothing
			End Get
		End Property
		<ServiceProperty(ServiceSearchMode.PreferParents)> _
		Protected Overridable ReadOnly Property DocumentManagerService() As IDocumentManagerService
			Get
				Return Nothing
			End Get
		End Property

		Protected Overridable Sub OnParameterChanged(ByVal parameter As Object)
			If Entity IsNot Nothing AndAlso IsNew() Then
				Remove(Entity)
			End If
			Dim detailEntity As IDetailEntityInfo = TryCast(parameter, IDetailEntityInfo)
			If detailEntity IsNot Nothing Then
				Entity = CreateEntity()
				TryInitializeDetail(detailEntity)
			ElseIf TypeOf parameter Is TPrimaryKey Then
				Me.parameter_Renamed = parameter
				Entity = Repository.Find(CType(parameter, TPrimaryKey))
			ElseIf parameter Is Nothing Then
				Entity = CreateEntity()
			Else
				Entity = Nothing
			End If
		End Sub
		Protected Overridable Function CreateEntity() As TEntity
			Return Repository.Create()
		End Function

		<Display(AutoGenerateField := False)> _
		Public Sub Update()
			UpdateTitle()
			UpdateCommands()
		End Sub
		Private Sub UpdateTitle()
			If Entity Is Nothing Then
				title_Renamed = Nothing
			ElseIf IsNew() Then
				title_Renamed = GetTitleForNewEntity()
			Else
				title_Renamed = GetTitle(GetState() = EntityState.Modified)
			End If
			Me.RaisePropertyChanged(Function(x) x.Title)
		End Sub
		Protected Overridable Sub UpdateCommands()
            Me.RaiseCanExecuteChanged(Sub(x) x.Save())
            Me.RaiseCanExecuteChanged(Sub(x) x.Delete())
		End Sub
		<Command> _
		Public Function Save() As Boolean
			Try
				Dim isNewEntity As Boolean = IsNew()
				If (Not isNewEntity) Then
					UnitOfWork.Update(Entity)
				End If
				UnitOfWork.SaveChanges()
				If isNewEntity Then
					Messenger.Default.Send(New EntityMessage(Of TEntity)(Entity, EntityMessageType.Added))
				Else
					Messenger.Default.Send(New EntityMessage(Of TEntity)(Entity, EntityMessageType.Changed))
				End If

				Parameter = GetPrimaryKey(Entity)
				OnEntityChanged()
				Me.RaisePropertyChanged(Function(x) x.Entity)
				Return True
			Catch e As DbException
				MessageBoxService.Show(e.ErrorMessage, e.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error)
				Return False
			End Try
		End Function
		Public Overridable Function CanSave() As Boolean
			Return Entity IsNot Nothing AndAlso Not HasValidationErrors()
		End Function
		Public Sub Delete()
			If MessageBoxService.Show(String.Format(CommonResources.Confirmation_Delete, GetType(TEntity).Name), CommonResources.Confirmation_Caption, MessageBoxButton.YesNo) <> MessageBoxResult.Yes Then
				Return
			End If
			Try
				OnBeforeEntityDeleted(Entity)
				Repository.Remove(Entity)
				UnitOfWork.SaveChanges()
				Dim toMessage As TEntity = Entity
				Entity = Nothing
				Messenger.Default.Send(New EntityMessage(Of TEntity)(toMessage, EntityMessageType.Deleted))
				Close()
			Catch e As DbException
				MessageBoxService.Show(e.ErrorMessage, e.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error)
			End Try
		End Sub
		Protected Overridable Sub OnBeforeEntityDeleted(ByVal entity As TEntity)
		End Sub
		Public Function CanDelete() As Boolean
			Return Entity IsNot Nothing AndAlso Not IsNew()
		End Function
		Public Sub Close()
			If (Not TryClose()) OrElse DocumentManagerService Is Nothing Then
				Return
			End If
			OnClose()
			Dim document As IDocument = DocumentManagerService.FindDocument(Me)
			If document IsNot Nothing Then
				document.Close()
			End If
		End Sub
		Protected Overridable Sub OnClose()
			Messenger.Default.Unregister(Me)
		End Sub
		Protected Overridable Function TryClose() As Boolean
			If HasValidationErrors() Then
				Dim warningResult As MessageBoxResult = MessageBoxService.Show(CommonResources.Warning_SomeFieldsContainInvalidData, CommonResources.Warning_Caption, MessageBoxButton.OKCancel)
				Return warningResult = MessageBoxResult.OK
			End If
			If (Not NeedSave()) Then
				Return True
			End If
			Dim result As MessageBoxResult = MessageBoxService.Show(CommonResources.Confirmation_Save, CommonResources.Confirmation_Caption, MessageBoxButton.YesNoCancel)
			Dim close As Boolean
			If result = MessageBoxResult.Yes Then
				close = Save()
			Else
				close = result <> MessageBoxResult.Cancel
			End If
			If close Then
				OnClose()
			End If
			Return close
		End Function
		Protected Function IsNew() As Boolean
			Return GetState() = EntityState.Added
		End Function
		Protected Function NeedSave() As Boolean
			If Entity Is Nothing Then
				Return False
			End If
			Dim state As EntityState = GetState()
			Return state = EntityState.Modified OrElse state = EntityState.Added
		End Function
		Protected Overridable Function HasValidationErrors() As Boolean
			Return False
		End Function
		Private Function GetTitle(ByVal entityModified As Boolean) As String
			If entityModified Then
				Return GetTitle() + CommonResources.Entity_Changed
			Else
				Return GetTitle()
			End If
		End Function
		Protected Overridable Function GetTitleForNewEntity() As String
			Return GetType(TEntity).Name + CommonResources.Entity_New
		End Function
		Protected Overridable Function GetTitle() As String
			Return GetType(TEntity).Name & " " & Convert.ToString(GetPrimaryKey(Entity))
		End Function

		Protected Overridable Function TryInitializeDetail(ByVal detailEntityInfo As IDetailEntityInfo) As Boolean
			Return True
		End Function
		Protected Function TryInitializeDetail(Of TDetailEntity As Class, TDetailEntityKey)(ByVal parameter As Object, ByVal detailsRepository As IRepository(Of TDetailEntity, TDetailEntityKey), ByVal assignAction As Action(Of TEntity, TDetailEntity)) As Boolean
			Dim detailEntityInfo As DetailEntityInfo(Of TDetailEntity) = TryCast(parameter, DetailEntityInfo(Of TDetailEntity))
			If detailEntityInfo Is Nothing Then
				Return False
			End If
			Dim key As TDetailEntityKey = CType(detailEntityInfo.DetailEntityKey, TDetailEntityKey)
			Dim detailEntity As TDetailEntity = detailsRepository.GetEntities().Where(ExpressionHelper.GetValueEqualsExpression(detailsRepository.GetPrimaryKeyExpression, key)).FirstOrDefault()
			If detailEntity IsNot Nothing Then
				assignAction(Entity, detailEntity)
				Me.RaisePropertyChanged(Function(x) x.Entity)
			End If
			Return True
		End Function

		Protected Function GetState() As EntityState
			Try
				Return UnitOfWork.GetState(Entity)
			Catch e1 As InvalidOperationException
				If TypeOf Parameter Is TPrimaryKey Then
					Repository.SetPrimaryKey(Entity, CType(Parameter, TPrimaryKey))
				End If
				Return UnitOfWork.GetState(Entity)
			End Try
		End Function
		Private Sub Remove(ByVal entity As TEntity)
			If Repository.HasPrimaryKey(entity) Then
				entity = Repository.Find(GetPrimaryKey(entity))
			End If
			Repository.Remove(Me.Entity)
		End Sub
		Protected Overridable Function GetPrimaryKey(ByVal entity As TEntity) As TPrimaryKey
			Return Me.Repository.GetPrimaryKey(entity)
		End Function

		Protected Sub DestroyDocument(ByVal document As IDocument)
			If document IsNot Nothing Then
				document.Close()
			End If
		End Sub
		Protected Overridable Function CreateLookUpCollectionViewModel(Of TDetailEntity As Class, TDetailPrimaryKey)(ByVal getRepositoryFunc As Func(Of TUnitOfWork, IRepository(Of TDetailEntity, TDetailPrimaryKey)), ByVal filterExpression As Expression(Of Func(Of TDetailEntity, Boolean)), ByVal masterEntityKey As TPrimaryKey) As LookUpCollectionViewModel(Of TEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork)
			Dim lookUpViewModel = ViewModelSource.Create(Function() New LookUpCollectionViewModel(Of TEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork)(UnitOfWorkFactory, getRepositoryFunc, filterExpression, masterEntityKey))
			lookUpViewModel.SetParentViewModel(Me)
			Return lookUpViewModel
		End Function
		Protected Overridable Function CreateReadOnlyLookUpCollectionViewModel(Of TDetailEntity As Class)(ByVal getRepositoryFunc As Func(Of TUnitOfWork, IReadOnlyRepository(Of TDetailEntity)), ByVal filterExpression As Expression(Of Func(Of TDetailEntity, Boolean))) As ReadOnlyLookUpCollectionViewModel(Of TEntity, TDetailEntity, TUnitOfWork)
			Dim lookUpViewModel = ViewModelSource.Create(Function() New ReadOnlyLookUpCollectionViewModel(Of TEntity, TDetailEntity, TUnitOfWork)(UnitOfWorkFactory, getRepositoryFunc, filterExpression))
			lookUpViewModel.SetParentViewModel(Me)
			Return lookUpViewModel
		End Function
		Protected Overridable Function GetLookUpEntities(Of TLookUpEntity As Class)(ByVal repository As IReadOnlyRepository(Of TLookUpEntity)) As IList(Of TLookUpEntity)
			repository.GetEntities().Load()
			Return repository.Local
		End Function
		#Region "ISupportParameter"
		Private Shared ReadOnly NotSetParameter As Object = New Object()
		Private parameter_Renamed As Object = NotSetParameter

		Private Property Parameter() As Object
			Get
				Return If(Object.Equals(parameter_Renamed, NotSetParameter), Nothing, parameter_Renamed)
			End Get
			Set(ByVal value As Object)
				If Object.Equals(parameter_Renamed, value) Then
					Return
				End If
				OnParameterChanged(value)
			End Set
		End Property
		Private Property ISupportParameter_Parameter() As Object Implements ISupportParameter.Parameter
			Get
				Return Parameter
			End Get
			Set(ByVal value As Object)
				Parameter = value
			End Set
		End Property
		#End Region
		#Region "IDocumentViewModel"
		Private ReadOnly Property IDocumentViewModel_Title() As Object Implements IDocumentViewModel.Title
			Get
				Return Title
			End Get
		End Property
		Private Function IDocumentViewModel_Close() As Boolean Implements IDocumentViewModel.Close
			Return TryClose()
		End Function
		#End Region
		#Region "ISingleObjectViewModel"
		Private ReadOnly Property ISingleObjectViewModel_Entity() As TEntity Implements ISingleObjectViewModel(Of TEntity, TPrimaryKey).Entity
			Get
				Return Entity
			End Get
		End Property
		Private ReadOnly Property ISingleObjectViewModel_PrimaryKey() As TPrimaryKey Implements ISingleObjectViewModel(Of TEntity, TPrimaryKey).PrimaryKey
			Get
				Return GetPrimaryKey(Entity)
			End Get
		End Property
		#End Region
	End Class
End Namespace