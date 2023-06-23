
	$(document).ready(function () {
		$(function () {
			$(document).on('click', '#btnSave',
				function (event) {
					event.preventDefault();
					const form = $('#form-comment-add');
					const url = '/Comment/Add/';
					const dataToSend = form.serialize();
					$.post(url, dataToSend).done(function (data) {
						const model = jQuery.parseJSON(data);
						console.log(model);
						if (model.ResultStatus === 0) {
							Swal.fire(
								'!',
								`adlı kategori başarıyla silinmiştir.`,
								'success'
							)
							else {
								Swal.fire({
									icon: 'error',
									title: 'Başarısız işlem!',
									text: `Başarısız`
								})
							}
							/*	$("#result").fadeIn(data);*/
							toastr(commentAddData);
						}
					);
				}
			);
		});

	});

