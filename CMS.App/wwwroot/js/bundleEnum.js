
function getEnumKey(object, value) {
    return Object.keys(object).find(key => object[key] === value);
}
function getEnumValue(object, key) {
    return Object.values(object).find(value => object[key] === value);
}

OrderType = {
    '0': 'Unknown',
    '1': 'سفارش',
    '2': 'درحال بررسی',
    '3': 'آماده سازی',
    '20': 'ارسال',
    '50': 'تحویل',
    '200': 'بازگشت',
    '201': 'لغو',
    '202': 'انصراف'
}