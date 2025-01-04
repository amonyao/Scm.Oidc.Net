var key = '';
var type = eval($("#type").val());

function isValidEmail(val) {
	return val && /^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/.test(val);
}

function isValidPhone(val) {
	return val && /^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/.test(val);
}

function isValidSms(val) {
	return val && /^\d{6}$/.test(val);
}

function uuid(length) {
	if (!length) {
		length = 32;
	}
	const num = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	let str = "";
	for (let i = 0; i < length; i++) {
		str += num.charAt(Math.floor(Math.random() * num.length));
	}
	return str;
}

function notice(notice) {
	$('#notice').html(notice);
}

function sendSms() {
	if (type == 1) {
		sendPhone();
		return;
	}
	if (type == 2) {
		sendEmail();
		return;
	}
}

function login() {
	if (type == 1) {
		loginPhone();
		return;
	}
	if (type == 2) {
		loginEmail();
		return;
	}
}

function sendEmail() {
	var email = $("#email").val();
	if (!email) {
		notice('请输入电子邮件！');
		return;
	}
	if (!isValidEmail(email)) {
		notice('无效的电子邮件格式！');
		return;
	}

	if (!key) {
		key = uuid();
	}

	$('#send').html('正在发送……');
	$.ajax({
		url: "/home/sendSms",
		method: 'get',
		data: {
			'type': type,
			'code': email,
			'key': key
		},
		success: function (result) {
			if (result && result.success) {
				notice('验证码已发送，请注意查收邮件！');
				countDown(60, $('#send'));
				$('#check').attr('disabled', false);
				return;
			}

			notice(result.message);
			$('#send').html('-_-');
		},
		error: function (result) {
			notice(result);
		}
	});
}

function loginEmail() {
	var email = $("#email").val();
	if (!email) {
		notice('请输入电子邮件！');
		return;
	}
	if (!isValidEmail(email)) {
		notice('无效的电子邮件格式！');
		return;
	}

	var sms = $('#sms').val();
	if (!sms) {
		notice('请输入验证码！');
		return;
	}
	if (!isValidSms(sms)) {
		notice('无效的验证码！');
		return;
	}

	$('#key').val(key);
	$('#code').val(email);
	$('#form1').submit();
	//$('#check').attr("disabled", "true");
	//$.ajax({
	//	url: "/OAuth/CheckSms",
	//	method: 'post',
	//	data: {
	//		'code': email,
	//		'key': key,
	//		'sms': sms
	//	},
	//	success: function (result) {
	//		if (result && result.success) {
	//			window.location.href = "/user/index";
	//			return;
	//		}

	//		notice(result.message);
	//		$("#login").removeAttr("disabled")
	//	},
	//	error: function (result) {
	//		notice(result);
	//	}
	//});
}

function sendPhone() {
}

function loginPhone() {
}

var time = 0;
function countDown(steps, element) {
	if (time > 0 || !steps || !element) {
		return;
	}

	time = steps - 1;
	if (time < 1) {
		element.attr("disabled", "true");
		return;
	}
	element.attr("disabled", "true").html("重新发送(" + time + ")");

	var obj = setInterval(function () {
		if (time <= 0) {
			element.removeAttr("disabled").html("重新发送");
			clearInterval(obj);
		} else {
			element.attr("disabled", "true").html("重新发送(" + time + ")");
			time--;
		}
	}, 1000);
}

$().ready(function () {
	$('#send').click(function () {
		sendSms();
	});
	$('#check').click(function () {
		if (!$("#form1").validate()) {
			return;
		}

		login();
		return false;
	});
});