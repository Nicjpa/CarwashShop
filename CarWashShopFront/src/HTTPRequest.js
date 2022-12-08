const url = "https://localhost:7168/api/";

export const HTTPRequest = async (props) => {
  const controller = props.controller;
  const action = props.action;
  const param = props.params === null ? "" : props.params;

  const response = await fetch(url + controller + action + param, {
    method: props.method,
    body: props.body,
    headers: props.headers,
  });

  let data;
  let numOfPages;
  let totalCountOfItems;
  if (!response.ok) {
    const message = await response.json();

    let messages = null;
    if (typeof message.value === "object") {
      messages = [];
      messages.push("split");
      message.value.forEach((x) => messages.push(`${x.description}*`));
    } else {
      messages = message.value;
    }
    throw new Error(messages);
  }

  if (props.method !== "DELETE") {
    data = await response.json();

    numOfPages = response.headers.get("NumberOfPages");
    totalCountOfItems = response.headers.get("totalamountofitems");
  }

  if (props.method === "DELETE") {
    const message = await response.json();
    data = message.value;
  }

  return {
    data: data,
    numOfPages: numOfPages,
    totalCountOfItems: totalCountOfItems,
  };
};
