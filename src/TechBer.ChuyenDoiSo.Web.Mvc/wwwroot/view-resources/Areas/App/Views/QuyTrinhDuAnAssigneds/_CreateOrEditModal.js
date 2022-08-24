(function ($) {
    app.modals.CreateOrEditQuyTrinhDuAnAssignedModal = function () {

        var _quyTrinhDuAnAssignedsService = abp.services.app.quyTrinhDuAnAssigneds;

        var _modalManager;
        var _$quyTrinhDuAnAssignedInformationForm = null;

		        var _QuyTrinhDuAnAssignedloaiDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAnAssigneds/LoaiDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAnAssigneds/_QuyTrinhDuAnAssignedLoaiDuAnLookupTableModal.js',
            modalClass: 'LoaiDuAnLookupTableModal'
        });        var _QuyTrinhDuAnAssignedquyTrinhDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAnAssigneds/QuyTrinhDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAnAssigneds/_QuyTrinhDuAnAssignedQuyTrinhDuAnLookupTableModal.js',
            modalClass: 'QuyTrinhDuAnLookupTableModal'
        });        var _QuyTrinhDuAnAssignedquyTrinhDuAnAssignedLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAnAssigneds/QuyTrinhDuAnAssignedLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAnAssigneds/_QuyTrinhDuAnAssignedQuyTrinhDuAnAssignedLookupTableModal.js',
            modalClass: 'QuyTrinhDuAnAssignedLookupTableModal'
        });        var _QuyTrinhDuAnAssignedduAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAnAssigneds/DuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAnAssigneds/_QuyTrinhDuAnAssignedDuAnLookupTableModal.js',
            modalClass: 'DuAnLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$quyTrinhDuAnAssignedInformationForm = _modalManager.getModal().find('form[name=QuyTrinhDuAnAssignedInformationsForm]');
            _$quyTrinhDuAnAssignedInformationForm.validate();
        };

		          $('#OpenLoaiDuAnLookupTableButton').click(function () {

            var quyTrinhDuAnAssigned = _$quyTrinhDuAnAssignedInformationForm.serializeFormToObject();

            _QuyTrinhDuAnAssignedloaiDuAnLookupTableModal.open({ id: quyTrinhDuAnAssigned.loaiDuAnId, displayName: quyTrinhDuAnAssigned.loaiDuAnName }, function (data) {
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=loaiDuAnName]').val(data.displayName); 
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=loaiDuAnId]').val(data.id); 
            });
        });
		
		$('#ClearLoaiDuAnNameButton').click(function () {
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=loaiDuAnName]').val(''); 
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=loaiDuAnId]').val(''); 
        });
		
        $('#OpenQuyTrinhDuAnLookupTableButton').click(function () {

            var quyTrinhDuAnAssigned = _$quyTrinhDuAnAssignedInformationForm.serializeFormToObject();

            _QuyTrinhDuAnAssignedquyTrinhDuAnLookupTableModal.open({ id: quyTrinhDuAnAssigned.quyTrinhDuAnId, displayName: quyTrinhDuAnAssigned.quyTrinhDuAnName }, function (data) {
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=quyTrinhDuAnName]').val(data.displayName); 
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=quyTrinhDuAnId]').val(data.id); 
            });
        });
		
		$('#ClearQuyTrinhDuAnNameButton').click(function () {
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=quyTrinhDuAnName]').val(''); 
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=quyTrinhDuAnId]').val(''); 
        });
		
        $('#OpenQuyTrinhDuAnAssignedLookupTableButton').click(function () {

            var quyTrinhDuAnAssigned = _$quyTrinhDuAnAssignedInformationForm.serializeFormToObject();

            _QuyTrinhDuAnAssignedquyTrinhDuAnAssignedLookupTableModal.open({ id: quyTrinhDuAnAssigned.parentId, displayName: quyTrinhDuAnAssigned.quyTrinhDuAnAssignedName }, function (data) {
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=quyTrinhDuAnAssignedName]').val(data.displayName); 
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=parentId]').val(data.id); 
            });
        });
		
		$('#ClearQuyTrinhDuAnAssignedNameButton').click(function () {
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=quyTrinhDuAnAssignedName]').val(''); 
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=parentId]').val(''); 
        });
		
        $('#OpenDuAnLookupTableButton').click(function () {

            var quyTrinhDuAnAssigned = _$quyTrinhDuAnAssignedInformationForm.serializeFormToObject();

            _QuyTrinhDuAnAssignedduAnLookupTableModal.open({ id: quyTrinhDuAnAssigned.duAnId, displayName: quyTrinhDuAnAssigned.duAnName }, function (data) {
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=duAnName]').val(data.displayName); 
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=duAnId]').val(data.id); 
            });
        });
		
		$('#ClearDuAnNameButton').click(function () {
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=duAnName]').val(''); 
                _$quyTrinhDuAnAssignedInformationForm.find('input[name=duAnId]').val(''); 
        });
		


        this.save = function () {
            if (!_$quyTrinhDuAnAssignedInformationForm.valid()) {
                return;
            }
            if ($('#QuyTrinhDuAnAssigned_LoaiDuAnId').prop('required') && $('#QuyTrinhDuAnAssigned_LoaiDuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('LoaiDuAn')));
                return;
            }
            if ($('#QuyTrinhDuAnAssigned_QuyTrinhDuAnId').prop('required') && $('#QuyTrinhDuAnAssigned_QuyTrinhDuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('QuyTrinhDuAn')));
                return;
            }
            if ($('#QuyTrinhDuAnAssigned_ParentId').prop('required') && $('#QuyTrinhDuAnAssigned_ParentId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('QuyTrinhDuAnAssigned')));
                return;
            }
            if ($('#QuyTrinhDuAnAssigned_DuAnId').prop('required') && $('#QuyTrinhDuAnAssigned_DuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DuAn')));
                return;
            }

            var quyTrinhDuAnAssigned = _$quyTrinhDuAnAssignedInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _quyTrinhDuAnAssignedsService.createOrEdit(
				quyTrinhDuAnAssigned
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditQuyTrinhDuAnAssignedModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);