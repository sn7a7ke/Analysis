export function getNextElement(arr, el) {
    let idx = arr.findIndex(x => x == el);
    let nextIdx = (idx + 1) % arr.length;
    return arr[nextIdx];
}

export function getKey(country, dataType) {
    return country + dataType;
}