(function ($) {
    app.modals.CreateOrEditTraoDoiVanBanDuAnModal = function () {

        var _traoDoiVanBanDuAnsService = abp.services.app.traoDoiVanBanDuAns;

        var _modalManager;
        var _$traoDoiVanBanDuAnInformationForm = null;

		        var _TraoDoiVanBanDuAnuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TraoDoiVanBanDuAns/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TraoDoiVanBanDuAns/_TraoDoiVanBanDuAnUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });        var _TraoDoiVanBanDuAnvanBanDuAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TraoDoiVanBanDuAns/VanBanDuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TraoDoiVanBanDuAns/_TraoDoiVanBanDuAnVanBanDuAnLookupTableModal.js',
            modalClass: 'VanBanDuAnLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$traoDoiVanBanDuAnInformationForm = _modalManager.getModal().find('form[name=TraoDoiVanBanDuAnInformationsForm]');
            _$traoDoiVanBanDuAnInformationForm.validate();
        };

		          $('#OpenUserLookupTableButton').click(function () {

            var traoDoiVanBanDuAn = _$traoDoiVanBanDuAnInformationForm.serializeFormToObject();

            _TraoDoiVanBanDuAnuserLookupTableModal.open({ id: traoDoiVanBanDuAn.userId, displayName: traoDoiVanBanDuAn.userName }, function (data) {
                _$traoDoiVanBanDuAnInformationForm.find('input[name=userName]').val(data.displayName); 
                _$traoDoiVanBanDuAnInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$traoDoiVanBanDuAnInformationForm.find('input[name=userName]').val(''); 
                _$traoDoiVanBanDuAnInformationForm.find('input[name=userId]').val(''); 
        });
		
        $('#OpenVanBanDuAnLookupTableButton').click(function () {

            var traoDoiVanBanDuAn = _$traoDoiVanBanDuAnInformationForm.serializeFormToObject();

            _TraoDoiVanBanDuAnvanBanDuAnLookupTableModal.open({ id: traoDoiVanBanDuAn.vanBanDuAnId, displayName: traoDoiVanBanDuAn.vanBanDuAnName }, function (data) {
                _$traoDoiVanBanDuAnInformationForm.find('input[name=vanBanDuAnName]').val(data.displayName); 
                _$traoDoiVanBanDuAnInformationForm.find('input[name=vanBanDuAnId]').val(data.id); 
            });
        });
		
		$('#ClearVanBanDuAnNameButton').click(function () {
                _$traoDoiVanBanDuAnInformationForm.find('input[name=vanBanDuAnName]').val(''); 
                _$traoDoiVanBanDuAnInformationForm.find('input[name=vanBanDuAnId]').val(''); 
        });
		


        this.save = function () {
            if (!_$traoDoiVanBanDuAnInformationForm.valid()) {
                return;
            }
            if ($('#TraoDoiVanBanDuAn_UserId').prop('required') && $('#TraoDoiVanBanDuAn_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#TraoDoiVanBanDuAn_VanBanDuAnId').prop('required') && $('#TraoDoiVanBanDuAn_VanBanDuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('VanBanDuAn')));
                return;
            }

            var traoDoiVanBanDuAn = _$traoDoiVanBanDuAnInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _traoDoiVanBanDuAnsService.createOrEdit(
				traoDoiVanBanDuAn
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditTraoDoiVanBanDuAnModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);