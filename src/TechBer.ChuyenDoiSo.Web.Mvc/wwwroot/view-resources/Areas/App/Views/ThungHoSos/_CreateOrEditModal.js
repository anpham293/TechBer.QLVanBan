(function ($) {
    app.modals.CreateOrEditThungHoSoModal = function () {

        var _thungHoSosService = abp.services.app.thungHoSos;

        var _modalManager;
        var _$thungHoSoInformationForm = null;

		        var _ThungHoSodayKeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ThungHoSos/DayKeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ThungHoSos/_ThungHoSoDayKeLookupTableModal.js',
            modalClass: 'DayKeLookupTableModal'
        });        var _ThungHoSoduAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ThungHoSos/DuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ThungHoSos/_ThungHoSoDuAnLookupTableModal.js',
            modalClass: 'DuAnLookupTableModal',
            modalSize: 'modal-xl'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$thungHoSoInformationForm = _modalManager.getModal().find('form[name=ThungHoSoInformationsForm]');
            _$thungHoSoInformationForm.validate();
        };

		          $('#OpenDayKeLookupTableButton').click(function () {

            var thungHoSo = _$thungHoSoInformationForm.serializeFormToObject();

            _ThungHoSodayKeLookupTableModal.open({ id: thungHoSo.dayKeId, displayName: thungHoSo.dayKeMaSo }, function (data) {
                _$thungHoSoInformationForm.find('input[name=dayKeMaSo]').val(data.displayName); 
                _$thungHoSoInformationForm.find('input[name=dayKeId]').val(data.id); 
            });
        });
		
		$('#ClearDayKeMaSoButton').click(function () {
                _$thungHoSoInformationForm.find('input[name=dayKeMaSo]').val(''); 
                _$thungHoSoInformationForm.find('input[name=dayKeId]').val(''); 
        });
		
        $('#OpenDuAnLookupTableButton').click(function () {

            var thungHoSo = _$thungHoSoInformationForm.serializeFormToObject();

            _ThungHoSoduAnLookupTableModal.open({ id: thungHoSo.duAnId, displayName: thungHoSo.duAnName }, function (data) {
                _$thungHoSoInformationForm.find('input[name=duAnName]').val(data.displayName); 
                _$thungHoSoInformationForm.find('input[name=duAnId]').val(data.id); 
            });
        });
		
		$('#ClearDuAnNameButton').click(function () {
                _$thungHoSoInformationForm.find('input[name=duAnName]').val(''); 
                _$thungHoSoInformationForm.find('input[name=duAnId]').val(''); 
        });
		


        this.save = function () {
            if (!_$thungHoSoInformationForm.valid()) {
                return;
            }
            if ($('#ThungHoSo_DayKeId').prop('required') && $('#ThungHoSo_DayKeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DayKe')));
                return;
            }
            if ($('#ThungHoSo_DuAnId').prop('required') && $('#ThungHoSo_DuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DuAn')));
                return;
            }

            var thungHoSo = _$thungHoSoInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _thungHoSosService.createOrEdit(
				thungHoSo
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditThungHoSoModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);