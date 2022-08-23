(function ($) {
    app.modals.CreateOrEditLoaiDuAnModal = function () {

        var _loaiDuAnsService = abp.services.app.loaiDuAns;

        var _modalManager;
        var _$loaiDuAnInformationForm = null;

		        var _LoaiDuAnorganizationUnitLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LoaiDuAns/OrganizationUnitLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LoaiDuAns/_LoaiDuAnOrganizationUnitLookupTableModal.js',
            modalClass: 'OrganizationUnitLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$loaiDuAnInformationForm = _modalManager.getModal().find('form[name=LoaiDuAnInformationsForm]');
            _$loaiDuAnInformationForm.validate();
        };

		          $('#OpenOrganizationUnitLookupTableButton').click(function () {

            var loaiDuAn = _$loaiDuAnInformationForm.serializeFormToObject();

            _LoaiDuAnorganizationUnitLookupTableModal.open({ id: loaiDuAn.organizationUnitId, displayName: loaiDuAn.organizationUnitDisplayName }, function (data) {
                _$loaiDuAnInformationForm.find('input[name=organizationUnitDisplayName]').val(data.displayName); 
                _$loaiDuAnInformationForm.find('input[name=organizationUnitId]').val(data.id); 
            });
        });
		
		$('#ClearOrganizationUnitDisplayNameButton').click(function () {
                _$loaiDuAnInformationForm.find('input[name=organizationUnitDisplayName]').val(''); 
                _$loaiDuAnInformationForm.find('input[name=organizationUnitId]').val(''); 
        });
		


        this.save = function () {
            if (!_$loaiDuAnInformationForm.valid()) {
                return;
            }
            if ($('#LoaiDuAn_OrganizationUnitId').prop('required') && $('#LoaiDuAn_OrganizationUnitId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('OrganizationUnit')));
                return;
            }

            var loaiDuAn = _$loaiDuAnInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _loaiDuAnsService.createOrEdit(
				loaiDuAn
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditLoaiDuAnModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);