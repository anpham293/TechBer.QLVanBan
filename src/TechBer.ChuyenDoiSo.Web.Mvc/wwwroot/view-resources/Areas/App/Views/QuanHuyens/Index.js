(function () {
    $(function () {

        var _$quanHuyensTable = $('#QuanHuyensTable');
        var _quanHuyensService = abp.services.app.quanHuyens;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyDanhMuc.QuanHuyen';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.QuanHuyens.Create'),
            edit: abp.auth.hasPermission('Pages.QuanHuyens.Edit'),
            'delete': abp.auth.hasPermission('Pages.QuanHuyens.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuanHuyens/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuanHuyens/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditQuanHuyenModal'
        });       

		 var _viewQuanHuyenModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuanHuyens/ViewquanHuyenModal',
            modalClass: 'ViewQuanHuyenModal'
        });

		        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
		        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$quanHuyensTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _quanHuyensService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#QuanHuyensTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					maFilter: $('#MaFilterId').val(),
					tinhThanhNameFilter: $('#TinhThanhNameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewQuanHuyenModal.open({ id: data.record.quanHuyen.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.quanHuyen.id });                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.quanHuyen.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteQuanHuyen(data.record.quanHuyen);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "quanHuyen.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "quanHuyen.ma",
						 name: "ma"   
					},
					{
						targets: 3,
						 data: "tinhThanhName" ,
						 name: "tinhThanhFk.name" 
					}
            ]
        });

        function getQuanHuyens() {
            dataTable.ajax.reload();
        }

        function deleteQuanHuyen(quanHuyen) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _quanHuyensService.delete({
                            id: quanHuyen.id
                        }).done(function () {
                            getQuanHuyens(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewQuanHuyenButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _quanHuyensService
                .getQuanHuyensToExcel({
				filter : $('#QuanHuyensTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					maFilter: $('#MaFilterId').val(),
					tinhThanhNameFilter: $('#TinhThanhNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditQuanHuyenModalSaved', function () {
            getQuanHuyens();
        });

		$('#GetQuanHuyensButton').click(function (e) {
            e.preventDefault();
            getQuanHuyens();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getQuanHuyens();
		  }
		});
    });
})();