(function ($) {
    app.modals.CreateOrEditDuAnModal = function () {

        var _duAnsService = abp.services.app.duAns;

        var _modalManager;
        var _$duAnInformationForm = null;

		        var _DuAnloaiDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAns/LoaiDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DuAns/_DuAnLoaiDuAnLookupTableModal.js',
            modalClass: 'LoaiDuAnLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$duAnInformationForm = _modalManager.getModal().find('form[name=DuAnInformationsForm]');
            _$duAnInformationForm.validate();
        };

		          $('#OpenLoaiDuAnLookupTableButton').click(function () {

            var duAn = _$duAnInformationForm.serializeFormToObject();

            _DuAnloaiDuAnLookupTableModal.open({ id: duAn.loaiDuAnId, displayName: duAn.loaiDuAnName }, function (data) {
                _$duAnInformationForm.find('input[name=loaiDuAnName]').val(data.displayName); 
                _$duAnInformationForm.find('input[name=loaiDuAnId]').val(data.id); 
            });
        });
		
		$('#ClearLoaiDuAnNameButton').click(function () {
                _$duAnInformationForm.find('input[name=loaiDuAnName]').val(''); 
                _$duAnInformationForm.find('input[name=loaiDuAnId]').val(''); 
        });
		


        this.save = function () {
            if (!_$duAnInformationForm.valid()) {
                return;
            }
            if ($('#DuAn_LoaiDuAnId').prop('required') && $('#DuAn_LoaiDuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('LoaiDuAn')));
                return;
            }

            var duAn = _$duAnInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _duAnsService.createOrEdit(
				duAn
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDuAnModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);