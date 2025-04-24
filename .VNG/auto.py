import requests
import time
from datetime import datetime

# ✅ Đọc danh sách mã giftcode từ file
with open("code.txt", "r", encoding="utf-8") as f:
    giftcodes = [line.strip() for line in f if line.strip()]

# ✅ Thông tin người dùng (cần thay bằng của bạn)
user_info = {"userId":"3209577257987686400","profileId":"311923431b66e981","serverId":"66","gameCode":"946","roleId":"1801512955934724488","roleName":"slowq","level":""}

# ✅ Token lấy từ trình duyệt
AUTH_TOKEN = "YjRFOHlGalFSSzZMS29weVVPb3RVT3d4OGVQdVdsdFh4Um5qQjZkUUJDYz12clBSbm1obHBya1c2QDNYIyhZc0B5ZykjUURrbklPSGVsdk45R3p0WEJkZzJpaDgqanlMI2xUKjBVR2dtOXVLaENSQ3laKkRlN1dSd2pQd01HbTArMTM2NTAxMDMyOTIzMzU4MDAzMg=="

# ✅ API URL
url = "https://vgrapi-sea.vnggames.com/coordinator/api/v1/code/redeem"

headers = {
    "accept": "application/json, text/plain, */*",
    "authorization": AUTH_TOKEN,
    "content-type": "application/json",
    "origin": "https://giftcode.vnggames.com",
    "referer": "https://giftcode.vnggames.com/",
    "user-agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
    "x-client-region": "VN"
}

# ✅ Mở file result.txt để ghi kết quả
with open("result.txt", "w", encoding="utf-8") as result_file:
    result_file.write(f"Kết quả nhập giftcode - {datetime.now()}\n\n")
    
    for code in giftcodes:
        payload = {**user_info, "code": code}
        try:
            response = requests.post(url, json=payload, headers=headers)
            result = response.json()
            # Tùy API, bạn có thể kiểm tra message, statusCode, v.v.
            status_msg = result.get("message") or str(result)
            log = f"[{code}] → {status_msg}"
        except Exception as e:
            log = f"[{code}] → LỖI: {e}"

        print(log)
        result_file.write(log + "\n")
        time.sleep(1)  # Đợi 1 giây để tránh spam quá nhanh
