import { useState, useEffect, useRef } from "react";
import { makeStyles } from "@material-ui/core/styles";

import {
  Collapse,
  Grid,
  Typography,
  Button,
  IconButton,
  TextField,
  Tooltip,
} from "@material-ui/core";

import StorefrontIcon from "@material-ui/icons/Storefront";
import RoomIcon from "@material-ui/icons/Room";
import AccessTimeIcon from "@material-ui/icons/AccessTime";
import LocalCarWashIcon from "@material-ui/icons/LocalCarWash";
import CloseSharpIcon from "@material-ui/icons/CloseSharp";
import EditSharpIcon from "@material-ui/icons/EditSharp";
import CheckSharpIcon from "@material-ui/icons/CheckSharp";
import DeleteSharpIcon from "@material-ui/icons/DeleteSharp";
import BuildOutlinedIcon from "@material-ui/icons/BuildOutlined";
import MonetizationOnOutlinedIcon from "@material-ui/icons/MonetizationOnOutlined";

import Pagination from "../Pagination";
import { HTTPRequest } from "../../HTTPRequest";
import ShopModal from "../../UI/Modals/ShopModal";

const useStyles = makeStyles((theme) => ({
  expand: {
    transform: "rotate(0deg)",
    marginLeft: "auto",
    color: "white",
    transition: theme.transitions.create("transform", {
      duration: theme.transitions.duration.shortest,
    }),
  },
  expandOpen: {
    transform: "rotate(180deg)",
  },
  container: {
    border: "7px solid white",
    borderRadius: "24px",
    backgroundColor: "rgba(0,0,50,0.55)",
    backdropFilter: "blur(10px)",
    transition: "0.12s linear",
  },
  cell: {
    width: "20%",
    alignItems: "center",
    justifyContent: "center",
    padding: "0",
  },
  cellTitle: {
    width: "20%",
    flexDirection: "column",
    alignItems: "center",
  },
  titleIcon: {
    fontSize: 50,
    color: "white",
    filter: "drop-shadow(0 0 7px dodgerblue)",
  },
  titleText: {
    color: "dodgerblue",
    fontFamily: "Orbitron",
    fontWeight: 700,
    fontSize: 24,
  },
  shopInfo: {
    color: "white",
    fontFamily: "Orbitron",
    fontWeight: 100,
    userSelect: "none",
    textAlign: "center",
  },
  collapseWrapper: {
    width: "100%",
    borderTop: "2px dodgerblue dashed",
  },
  serviceCardTitles: {
    fontFamily: "Orbitron",
    color: "dodgerblue",
    fontWeight: 700,
    fontSize: 18,
  },
  serviceCardInfo: {
    fontFamily: "Orbitron",
    fontSize: 18,
    textShadow: "0 0 10px white",
  },
  editDeleteBtn: {
    color: "white",
    fontFamily: "Orbitron",
    border: "2px dashed dodgerblue",
    borderRadius: "100px",
    margin: "0.5em 0",
    textShadow: "0 0 10px dodgerblue",
    transition: "0.15s linear",
  },
  removalProcessIcon: {
    color: "white",
    fontFamily: "Orbitron",
    borderRadius: "100px",
    margin: "0.5em 0",
    textShadow: "0 0 10px dodgerblue",
    transition: "0.15s linear",
    border: "2px solid red",
    backgroundColor: "red",
    boxShadow: "0 0 20px red",
    cursor: "default",
    "&:hover": {
      backgroundColor: "red",
    },
  },

  editHover: {
    "&:hover": {
      border: "2px solid orange",
      backgroundColor: "orange",
      boxShadow: "0 0 14px orange",
    },
  },
  saveEditChanges: {
    border: "2px solid dodgerblue",
    "&:hover": {
      border: "2px solid limegreen",
      backgroundColor: "limegreen",
      boxShadow: "0 0 14px limegreen",
    },
  },
  cancelNewService: {
    border: "2px solid dodgerblue",
    "&:hover": {
      border: "2px solid red",
      backgroundColor: "red",
      boxShadow: "0 0 14px red",
    },
  },
  deleteHover: {
    "&:hover": {
      border: "2px solid red",
      backgroundColor: "red",
      boxShadow: "0 0 20px red",
    },
  },
  clickableRow: {
    cursor: "pointer",
    "&:hover": {
      backgroundColor: "rgba(0,0,0,0.4)",
    },
  },
  cardContainer: {
    border: "4px white solid",
    width: "450px",
    height: "auto",
    borderRadius: "20px",
    boxShadow: "0 0 15px transparent",
    transition: "0.15s linear",
    margin: "2em",
    outline: "1px solid dodgerblue",
    outlineOffset: "5px",
  },

  cardContainerEditMode: {
    border: "4px dodgerblue solid",
    outline: "7px solid white",
    boxShadow: "0 0 40px dodgerblue",
  },

  editInput: {
    transition: "0.5s linear",
    "& .Mui-focused .MuiOutlinedInput-notchedOutline": {
      border: "1px dashed dodgerblue",
    },
    "& .MuiOutlinedInput-multiline": {
      padding: 0,
      width: "18.5rem",
    },
    "& .MuiInputBase-input": {
      color: "white",
      fontWeight: 100,
      fontSize: "18px",
    },
    "& .MuiOutlinedInput-notchedOutline": {
      border: "1px dashed white",
      padding: 0,
      transition: "0.2s linear",
      borderRadius: 0,
    },
    "& .Mui-disabled .MuiOutlinedInput-notchedOutline": {
      borderColor: "transparent",
    },
    "& .MuiOutlinedInput-input": {
      padding: "0.2rem 0.4rem",
    },
    "& .MuiFormHelperText-contained": {
      color: "transparent",
      userSelect: "none",
      fontFamily: "Orbitron",
    },
    "& .MuiFormHelperText-root.Mui-error": {
      color: "red",
    },
  },

  description: {
    "& .Mui-disabled .MuiOutlinedInput-notchedOutline": {
      border: "2px solid dodgerblue",
      borderRadius: 0,
    },
  },

  name: {
    width: "13rem",
    "& .MuiOutlinedInput-notchedOutline": {
      borderRadius: "0 25px 25px 0",
    },
    "& .MuiOutlinedInput-input": {
      padding: "0.2rem 2px",
    },
  },

  priceTextAlign: {
    "& .MuiInputBase-input": {
      textAlign: "right",
      paddingRight: 0,
    },
    "& .MuiOutlinedInput-notchedOutline": {
      borderRadius: "25px 0 0 25px",
    },
  },
  CreateShopBtn: {
    padding: "1rem",
    width: "100%",
    color: "white",
    fontFamily: "Orbitron",
    fontSize: 40,
    fontWeight: 700,
    borderBottom: "4px dashed white",
    borderRadius: "16px 16px 0 0 ",
    transition: "0.2s linear",
    "&:hover": {
      backgroundColor: "dodgerblue",
      borderBottom: "4px solid dodgerblue",
      letterSpacing: "0.5em",
    },
    "&:active #storeIcon": {
      color: "dodgerblue",
    },
    "&:active": { backgroundColor: "white", color: "dodgerblue" },
  },
  createShopIcon: {
    color: "white",
    fontSize: 60,
    marginLeft: "0.4rem",
    transition: "0.2s linear",
  },
  addNewService: {
    transition: "0.15s linear",
    fontFamily: "Orbitron",
    fontSize: 24,
    fontWeight: 500,
    userSelect: "none",
  },
  newServiceIcon: {
    transition: "0.15s linear",
    fontSize: 70,
  },

  newServiceOpened: {
    border: "4px dodgerblue solid",
    outline: "7px solid white",
    boxShadow: "0 0 40px dodgerblue",
  },

  newServiceContainer: {
    cursor: "pointer",
    transition: "0.15s linear",
    "&:hover": {
      border: "4px dodgerblue solid",
      outline: "7px solid white",
      filter: "drop-shadow(0 0 7px dodgerblue)",
    },
    "&:hover #newServiceIcon": {
      color: "white",
      fontSize: 90,
    },
    "&:hover #newService": {
      color: "white",
      fontSize: 32,
    },
  },
  newServiceFields: {
    "& .MuiFormHelperText-contained": {
      color: "transparent",
      userSelect: "none",
      fontFamily: "Orbitron",
    },
    "& .MuiFormHelperText-root.Mui-error": {
      color: "red",
    },
  },
  noContentContainer: {
    padding: "5rem",
    borderTop: "7px solid white",
    justifyContent: "Center",
    alignItems: "center",
    flexDirection: "column",
    filter: "drop-shadow(0 0 0.3rem dodgerblue)",
  },
  noContentTitle: {
    width: "auto",
    color: "white",
    fontFamily: "Orbitron",
    fontSize: 50,
    fontWeight: 700,
    textShadow: "0 0 2px dodgerblue",
  },
}));

export const Row = (props) => {
  const css = useStyles();
  const shop = props.shop;
  const revenue = props.revenue;

  const shopOpeningTime =
    shop.openingTime.toString().length < 2
      ? `0${shop.openingTime}`
      : shop.openingTime;

  const shopClosingTime =
    shop.closingTime.toString().length < 2
      ? `0${shop.closingTime}`
      : shop.closingTime;

  const [expanded, setExpanded] = useState(false);
  const [newServiceForm, setNewServiceForm] = useState(false);

  const [newServiceNameError, setNewServiceNameError] = useState(false);
  const [newServicePriceError, setNewServicePriceError] = useState(false);
  const [newServiceDescError, setNewServiceDescError] = useState(false);

  const newServiceNameRef = useRef();
  const newServicePriceRef = useRef();
  const newServiceDescRef = useRef();

  const handleExpandClick = () => {
    setExpanded(!expanded);
  };

  const addServiceHandler = async () => {
    const newServiceName = newServiceNameRef.current.value;
    const newServicePrice = newServicePriceRef.current.value;
    const newServiceDesc = newServiceDescRef.current.value;

    const isNewServiceValid =
      newServiceName.trim().length > 0 &&
      newServiceDesc.trim().length > 0 &&
      !!parseFloat(newServicePrice) !== false &&
      parseFloat(newServicePrice) >= 1 &&
      parseFloat(newServicePrice) <= 100;

    if (isNewServiceValid) {
      const editServiceParams = {
        controller: "OwnerServices/",
        action: "AddNewServiceToShop",
        method: "POST",
        params: `?shopId=${shop.id}`,
        body: JSON.stringify({
          name: newServiceName,
          description: newServiceDesc,
          price: newServicePrice,
        }),
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + localStorage.getItem("token"),
        },
      };

      try {
        await HTTPRequest(editServiceParams);

        props.setInfoModalParams(() => {
          return {
            bool: true,
            modalTitle: "New Service",
            modalDesc: `You have added new service '${newServiceName}' to the '${shop.name}' shop.`,
            themeColor: "info",
          };
        });
        props.reloadPage();
        setNewServiceForm(false);
      } catch (error) {
        let errorMessage =
          error.message === "Failed to fetch"
            ? "No server response"
            : error.message.substring(0, 6) === "split,"
            ? error.message.substring(6, error.message.length - 1).split("*")
            : error.message;

        const modalTitle =
          errorMessage === "No server response"
            ? "Connection problem"
            : "Add new service failed";

        props.setInfoModalParams(() => {
          return {
            bool: true,
            modalTitle: modalTitle,
            modalDesc: errorMessage,
            themeColor: "error",
          };
        });
      }
    } else {
      newServiceName.trim().length === 0 && setNewServiceNameError(true);
      newServiceDesc.trim().length === 0 && setNewServiceDescError(true);
      if (
        !!parseFloat(newServicePrice) === false ||
        parseFloat(newServicePrice) < 1 ||
        parseFloat(newServicePrice) > 100
      ) {
        setNewServicePriceError(true);
      }
    }
  };

  const deleteShop = async () => {
    const deleteServiceParams = {
      controller: "OwnerShops/",
      action: "RemoveShop",
      method: "DELETE",
      params: null,
      body: JSON.stringify({
        shopId: `${shop.id}`,
        requestStatement: "",
      }),
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };
    props.setPromptModal((prevValues) => {
      return {
        ...prevValues,
        bool: false,
      };
    });
    try {
      const response = await HTTPRequest(deleteServiceParams);
      const messsage = response.data.includes("Removal request is already made")
        ? `Shop removal request is already made for the '${shop.name}'`
        : response.data;
      props.setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: "Shop removal",
          modalDesc: messsage,
          themeColor: "info",
        };
      });
      props.reloadPage();
    } catch (error) {
      let errorMessage =
        error.message === "Failed to fetch"
          ? "No server response"
          : error.message.substring(0, 6) === "split,"
          ? error.message.substring(6, error.message.length - 1).split("*")
          : error.message;

      const modalTitle =
        errorMessage === "No server response"
          ? "Connection problem"
          : "Removal denied";

      props.setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: modalTitle,
          modalDesc: errorMessage,
          themeColor: "error",
        };
      });
    }
  };

  return (
    <Grid container item style={{ borderTop: "2px solid dodgerblue" }}>
      <Grid
        container
        item
        justifyContent="space-evenly"
        onClick={handleExpandClick}
        className={css.clickableRow}
        style={{ backgroundColor: `${expanded ? "rgba(0,0,0,0.4)" : ""}` }}
      >
        <Grid container item className={css.cell}>
          <Typography
            variant="h6"
            className={css.shopInfo}
            style={{
              transition: "0.08s linear",
              color: `${expanded ? "dodgerblue" : ""}`,
            }}
          >
            {shop.name}
          </Typography>
        </Grid>
        <Grid container item className={css.cell}>
          <Typography
            variant="h6"
            className={css.shopInfo}
            style={{
              transition: "0.08s linear",
              color: `${expanded ? "dodgerblue" : ""}`,
            }}
          >
            {shop.address}
          </Typography>
        </Grid>
        <Grid container item className={css.cell} style={{ width: "15%" }}>
          <Typography
            variant="h6"
            className={css.shopInfo}
            style={{
              transition: "0.08s linear",
              color: `${expanded ? "dodgerblue" : ""}`,
            }}
          >
            {shop.amountOfWashingUnits}
          </Typography>
        </Grid>
        <Grid container item className={css.cell} style={{ width: "15%" }}>
          <Typography
            variant="h6"
            className={css.shopInfo}
            style={{
              transition: "0.08s linear",
              color: `${expanded ? "dodgerblue" : ""}`,
            }}
          >
            {shopOpeningTime} - {shopClosingTime}
          </Typography>
        </Grid>
        <Grid container item className={css.cell} style={{ width: "15%" }}>
          <Typography
            variant="h6"
            className={css.shopInfo}
            style={{
              transition: "0.08s linear",
              color: `${expanded ? "dodgerblue" : ""}`,
            }}
          >
            {revenue.totalRevenue}
          </Typography>
        </Grid>
        <Grid
          container
          item
          spacing={2}
          className={css.cell}
          style={{ width: "15%" }}
        >
          <Grid item>
            <IconButton
              disableRipple
              className={`${css.editDeleteBtn} ${css.editHover}`}
              onClick={(e) => {
                e.stopPropagation();
                props.setShopFormOpen(() => {
                  return {
                    open: true,
                    edit: true,
                    shopInfo: {
                      id: shop.id,
                      name: shop.name,
                      advertisingDescription: "string",
                      amountOfWashingUnits: shop.amountOfWashingUnits,
                      address: shop.address,
                      openingTime: shop.openingTime,
                      closingTime: shop.closingTime,
                    },
                  };
                });
              }}
            >
              <EditSharpIcon style={{ fontSize: 34 }} />
            </IconButton>
          </Grid>
          {!shop.isInRemovalProcess && (
            <Grid item>
              <IconButton
                disableRipple
                onClick={(e) => {
                  e.stopPropagation();
                  props.setPromptModal(() => {
                    return {
                      bool: true,
                      title: "Delete shop",
                      body: (
                        <Grid
                          container
                          direction="column"
                          alignItems="center"
                          justifyContent="center"
                        >
                          <Grid container item justifyContent="center">
                            <Typography
                              style={{
                                color: "orange",
                                fontFamily: "Orbitron",
                                fontWeight: 500,
                                fontSize: 26,
                              }}
                            >{`'${shop.name}' shop is gonna be deleted!`}</Typography>
                          </Grid>
                          <Grid container item justifyContent="center">
                            <Typography
                              style={{
                                color: "orange",
                                fontFamily: "Orbitron",
                                fontWeight: 500,
                                fontSize: 26,
                              }}
                            >
                              Are you sure?
                            </Typography>
                          </Grid>
                        </Grid>
                      ),
                      deleteFunc: deleteShop,
                    };
                  });
                }}
                className={`${css.editDeleteBtn} ${css.deleteHover}`}
              >
                <DeleteSharpIcon style={{ fontSize: 34 }} />
              </IconButton>
            </Grid>
          )}
          {shop.isInRemovalProcess && (
            <Grid item>
              <Tooltip title="Under removal process" arrow>
                <IconButton
                  disableRipple
                  className={css.removalProcessIcon}
                  onClick={(e) => {
                    e.stopPropagation();
                  }}
                >
                  <DeleteSharpIcon style={{ fontSize: 34 }} />
                </IconButton>
              </Tooltip>
            </Grid>
          )}
        </Grid>
      </Grid>
      <Collapse
        component={Grid}
        container
        classes={{
          wrapper: css.collapseWrapper,
        }}
        style={{
          color: "white",
          backgroundColor: "rgba(0,0,0,0.4)",
        }}
        in={expanded}
        timeout="auto"
        unmountOnExit
      >
        <Grid
          container
          item
          style={{
            padding: "2em 0",
            justifyContent: "space-evenly",
            alignItems: "center",
          }}
        >
          {shop.services.map((x, i) => (
            <ServiceCard
              key={i}
              service={x}
              revenue={revenue.byServicesRevenue[i]}
              reloadPage={props.reloadPage}
              setInfoModalParams={props.setInfoModalParams}
              setPromptModal={props.setPromptModal}
            />
          ))}

          {newServiceForm ? (
            <Grid
              container
              item
              direction="column"
              justifyContent="space-evenly"
              className={`${css.cardContainer} ${
                newServiceForm && css.newServiceOpened
              }`}
              style={{ height: "22.6rem" }}
            >
              <Grid
                container
                item
                justifyContent="space-evenly"
                alignItems="center"
              >
                <TextField
                  inputRef={newServiceNameRef}
                  className={css.newServiceFields}
                  error={newServiceNameError}
                  onFocus={() => {
                    setNewServiceNameError(false);
                  }}
                  helperText="* Required"
                  style={{ width: "13rem" }}
                  inputProps={{ maxLength: 15 }}
                  variant="outlined"
                  label="Service Name"
                />
                <TextField
                  inputRef={newServicePriceRef}
                  className={css.newServiceFields}
                  error={newServicePriceError}
                  onFocus={() => {
                    setNewServicePriceError(false);
                  }}
                  helperText="* $(1-100)"
                  style={{ width: "6rem" }}
                  inputProps={{ maxLength: 5 }}
                  variant="outlined"
                  label="Price"
                  onKeyDown={(e) => {
                    if (
                      e.shiftKey ||
                      (e.keyCode !== 8 &&
                        e.keyCode !== 110 &&
                        e.keyCode !== 190 &&
                        (e.keyCode < 48 || e.keyCode > 57) &&
                        (e.keyCode < 96 || e.keyCode > 105))
                    ) {
                      e.preventDefault();
                    }
                  }}
                />
              </Grid>
              <Grid
                container
                item
                direction="column"
                justifyContent="center"
                alignItems="center"
              >
                <TextField
                  inputRef={newServiceDescRef}
                  className={css.newServiceFields}
                  error={newServiceDescError}
                  onFocus={() => {
                    setNewServiceDescError(false);
                  }}
                  helperText="* Required"
                  multiline
                  minRows={3}
                  maxRows={3}
                  style={{ width: "19.8rem" }}
                  inputProps={{ maxLength: 150 }}
                  variant="outlined"
                  label="Description"
                />
              </Grid>
              <Grid
                container
                item
                direction="column"
                justifyContent="center"
                alignItems="center"
              ></Grid>
              <Grid
                container
                item
                justifyContent="center"
                spacing={5}
                style={{ marginTop: "-3rem" }}
              >
                <Grid
                  container
                  item
                  direction="column"
                  justifyContent="center"
                  alignItems="center"
                  style={{ width: "auto" }}
                >
                  <IconButton
                    disableRipple
                    onClick={addServiceHandler}
                    className={`${css.editDeleteBtn} ${css.saveEditChanges}`}
                  >
                    <CheckSharpIcon style={{ fontSize: 34 }} />
                  </IconButton>
                </Grid>
                <Grid
                  container
                  item
                  direction="column"
                  justifyContent="center"
                  alignItems="center"
                  style={{ width: "auto" }}
                >
                  <IconButton
                    disableRipple
                    onClick={() => {
                      setNewServiceForm(false);
                      setNewServiceNameError(false);
                      setNewServicePriceError(false);
                      setNewServiceDescError(false);
                    }}
                    className={`${css.editDeleteBtn} ${css.cancelNewService}`}
                  >
                    <CloseSharpIcon style={{ fontSize: 34 }} />
                  </IconButton>
                </Grid>
              </Grid>
            </Grid>
          ) : (
            <Grid
              container
              item
              className={`${css.cardContainer} ${css.newServiceContainer}`}
              style={{ height: "22.6rem" }}
              onClick={() => {
                setNewServiceForm(true);
              }}
            >
              <Grid
                container
                item
                direction="column"
                justifyContent="center"
                alignItems="center"
              >
                <LocalCarWashIcon
                  id="newServiceIcon"
                  className={css.newServiceIcon}
                />
                <Typography id="newService" className={css.addNewService}>
                  ADD NEW SERVICE
                </Typography>
              </Grid>
            </Grid>
          )}
        </Grid>
      </Collapse>
    </Grid>
  );
};

export const ServiceCard = (props) => {
  const css = useStyles();
  const service = props.service;
  const revenue = props.revenue;
  const [serviceName, setServiceName] = useState(service.name);
  const [price, setPrice] = useState(`$${service.price}`);
  const [desc, setDesc] = useState(service.description);
  const [isEditDisabled, setIsEditDisabled] = useState(true);

  const [serviceNameError, setServiceNameError] = useState(false);
  const [servicePriceError, setServicePriceError] = useState(false);
  const [serviceDescError, setServiceDescError] = useState(false);

  useEffect(() => {
    if (isEditDisabled) {
      setServiceName(service.name);
      setDesc(service.description);
      setPrice(`$${service.price}`);
    }
  }, [service]);

  const editModeHandle = async () => {
    if (isEditDisabled) {
      setIsEditDisabled(false);
      setPrice(service.price);
    } else {
      const isServiceValid =
        serviceName.trim().length > 0 &&
        desc.trim().length > 0 &&
        !!parseFloat(price) !== false &&
        parseFloat(price) >= 1 &&
        parseFloat(price) <= 100;

      if (isServiceValid) {
        const editServiceParams = {
          controller: "OwnerServices/",
          action: `UpdateShopService?ServiceID=${service.id}`,
          method: "PUT",
          params: null,
          body: JSON.stringify({
            name: serviceName,
            description: desc,
            price: parseFloat(price),
          }),
          headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + localStorage.getItem("token"),
          },
        };
        try {
          await HTTPRequest(editServiceParams);

          props.reloadPage();
          props.setInfoModalParams(() => {
            return {
              bool: true,
              modalTitle: "Edit service",
              modalDesc: `Service '${serviceName}' edited sucessfully!`,
              themeColor: "info",
            };
          });
          setIsEditDisabled(true);
        } catch (error) {
          let errorMessage =
            error.message === "Failed to fetch"
              ? "No server response"
              : error.message.substring(0, 6) === "split,"
              ? error.message.substring(6, error.message.length - 1).split("*")
              : error.message;

          const modalTitle =
            errorMessage === "No server response"
              ? "Connection problem"
              : "Service edit fail";

          props.setInfoModalParams(() => {
            return {
              bool: true,
              modalTitle: modalTitle,
              modalDesc: errorMessage,
              themeColor: "error",
            };
          });
        }
      } else {
        serviceName.trim().length === 0 && setServiceNameError(true);
        desc.trim().length === 0 && setServiceDescError(true);
        if (
          !!parseFloat(price) === false ||
          parseFloat(price) < 1 ||
          parseFloat(price) > 100
        ) {
          setServicePriceError(true);
        }
      }
    }
  };

  const deleteService = async () => {
    const editServiceParams = {
      controller: "OwnerServices/",
      action: `RemoveService?ServiceID=${service.id}`,
      method: "DELETE",
      params: null,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };
    props.setPromptModal((prevValues) => {
      return {
        ...prevValues,
        bool: false,
      };
    });
    try {
      await HTTPRequest(editServiceParams);
      props.setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: "Service removed",
          modalDesc: `You have successfully removed ${serviceName} service from the shop!`,
          themeColor: "info",
        };
      });
      props.reloadPage();
    } catch (error) {
      let errorMessage =
        error.message === "Failed to fetch"
          ? "No server response"
          : error.message.substring(0, 6) === "split,"
          ? error.message.substring(6, error.message.length - 1).split("*")
          : error.message;

      const modalTitle =
        errorMessage === "No server response"
          ? "Connection problem"
          : "Removal denied";

      props.setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: modalTitle,
          modalDesc: errorMessage,
          themeColor: "error",
        };
      });
    }
  };

  const closeOrDeleteServiceHandle = () => {
    if (isEditDisabled) {
      props.setPromptModal(() => {
        return {
          bool: true,
          title: "Delete service",
          body: (
            <Grid
              container
              direction="column"
              alignItems="center"
              justifyContent="center"
            >
              <Grid container item justifyContent="center">
                <Typography
                  style={{
                    color: "orange",
                    fontFamily: "Orbitron",
                    fontWeight: 500,
                    fontSize: 26,
                  }}
                >{`'${service.name}' service is gonna be deleted!`}</Typography>
              </Grid>
              <Grid container item justifyContent="center">
                <Typography
                  style={{
                    color: "orange",
                    fontFamily: "Orbitron",
                    fontWeight: 500,
                    fontSize: 26,
                  }}
                >
                  Are you sure?
                </Typography>
              </Grid>
            </Grid>
          ),
          deleteFunc: deleteService,
        };
      });
    } else {
      setIsEditDisabled(true);
      setServiceName(service.name);
      setPrice(`$${service.price}`);
      setDesc(service.description);
      setServiceNameError(false);
      setServiceDescError(false);
      setServicePriceError(false);
    }
  };

  return (
    <Grid container direction="column" style={{ width: "auto" }}>
      <Grid
        container
        item
        className={`${css.cardContainer} ${
          !isEditDisabled && css.cardContainerEditMode
        }`}
      >
        <Grid
          container
          justifyContent="space-between"
          style={{
            padding: "1em 2em 0 2em",
          }}
        >
          <Grid
            container
            item
            style={{ width: "auto", flexDirection: "column" }}
          >
            <Typography className={css.serviceCardTitles}>
              Service Name
            </Typography>
            <TextField
              error={serviceNameError}
              onFocus={() => {
                setServiceNameError(false);
              }}
              helperText="* Required"
              inputProps={{ maxLength: 15 }}
              disabled={isEditDisabled}
              className={`${css.editInput} ${css.name}`}
              variant="outlined"
              value={serviceName}
              onChange={(e) => {
                setServiceName(e.target.value);
              }}
            />
          </Grid>
          <Grid
            container
            item
            style={{
              width: "auto",
              flexDirection: "column",
              alignItems: "flex-end",
            }}
          >
            <Typography className={css.serviceCardTitles}>Price</Typography>
            <TextField
              error={servicePriceError}
              onFocus={() => {
                setServicePriceError(false);
              }}
              helperText="* 1-100"
              inputProps={{ maxLength: 5 }}
              disabled={isEditDisabled}
              className={`${css.editInput} ${css.priceTextAlign}`}
              variant="outlined"
              style={{ width: "5rem" }}
              value={price}
              onChange={(e) => {
                parseFloat(e.target.value) > 100
                  ? e.preventDefault()
                  : setPrice(e.target.value);
              }}
              onKeyDown={(e) => {
                if (
                  e.shiftKey ||
                  (e.keyCode !== 8 &&
                    e.keyCode !== 110 &&
                    e.keyCode !== 190 &&
                    (e.keyCode < 48 || e.keyCode > 57) &&
                    (e.keyCode < 96 || e.keyCode > 105))
                ) {
                  e.preventDefault();
                }
              }}
            />
          </Grid>
        </Grid>

        <Grid
          container
          style={{
            padding: "0 2em",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <Typography
            className={css.serviceCardTitles}
            style={{ marginBottom: "0.3em" }}
          >
            Description
          </Typography>
          <Grid container item justifyContent="center">
            <TextField
              error={serviceDescError}
              onFocus={() => {
                setServiceDescError(false);
              }}
              helperText="* Required"
              inputProps={{ maxLength: 150 }}
              disabled={isEditDisabled}
              className={`${css.editInput} ${css.description}`}
              value={desc}
              onChange={(e) => {
                setDesc(e.target.value);
              }}
              multiline
              autoComplete="off"
              autoCorrect="off"
              variant="outlined"
              minRows={3}
              maxRows={3}
            />
          </Grid>
        </Grid>
        <Grid
          container
          item
          justifyContent="center"
          spacing={3}
          style={{ margin: "-1em 0 0 0" }}
        >
          <Grid
            container
            item
            direction="column"
            justifyContent="center"
            alignItems="center"
            style={{ width: "auto" }}
          >
            <IconButton
              disableRipple
              onClick={editModeHandle}
              className={`${css.editDeleteBtn} ${
                isEditDisabled ? css.editHover : css.saveEditChanges
              }`}
            >
              {isEditDisabled ? (
                <EditSharpIcon style={{ fontSize: 34 }} />
              ) : (
                <CheckSharpIcon style={{ fontSize: 34 }} />
              )}
            </IconButton>
          </Grid>
          <Grid
            container
            item
            direction="column"
            justifyContent="center"
            alignItems="center"
            style={{ width: "auto" }}
          >
            <IconButton
              disableRipple
              onClick={closeOrDeleteServiceHandle}
              className={`${css.editDeleteBtn} ${css.deleteHover}`}
              style={{ borderStyle: `${isEditDisabled ? "dashed" : "solid"}` }}
            >
              {isEditDisabled ? (
                <DeleteSharpIcon style={{ fontSize: 34 }} />
              ) : (
                <CloseSharpIcon style={{ fontSize: 34 }} />
              )}
            </IconButton>
          </Grid>
        </Grid>
        <Grid
          container
          style={{
            borderTop: "1px solid dodgerblue",
            padding: "1em 2em 1.5em 2em",
            justifyContent: "space-between",
          }}
        >
          <Grid
            container
            item
            direction="column"
            justifyContent="center"
            alignItems="center"
            style={{ width: "auto" }}
          >
            <Typography className={css.serviceCardTitles}>Bookings</Typography>
            <Typography className={css.serviceCardInfo}>
              {revenue.amountOfBookings}
            </Typography>
          </Grid>

          <Grid
            container
            item
            direction="column"
            justifyContent="center"
            alignItems="flex-end"
            style={{ width: "auto" }}
          >
            <Typography className={css.serviceCardTitles}>Revenue</Typography>
            <Typography className={css.serviceCardInfo}>
              {revenue.serviceRevenue}
            </Typography>
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
};

const OwnerShopTable = (props) => {
  const css = useStyles();
  const allShops = props.shops.data;
  const allRevenues = props.shops.revenue;
  const [open, setOpen] = useState({ open: false, edit: false });

  return (
    <>
      <ShopModal
        open={open}
        setOpen={setOpen}
        reloadPage={props.reloadPage}
        setInfoModalParams={props.setInfoModalParams}
      />
      <Grid container direction="column" className={css.container}>
        <Grid container item justifyContent="center" alignItems="center">
          <Button
            disableRipple
            className={css.CreateShopBtn}
            onClick={() => {
              setOpen({ open: true, edit: false });
            }}
          >
            CREATE NEW SHOP
            <StorefrontIcon id="storeIcon" className={css.createShopIcon} />
          </Button>
        </Grid>
        <Grid
          container
          style={{
            padding: "2.5em 0",
          }}
        >
          <Grid container item className={css.cellTitle}>
            <StorefrontIcon className={css.titleIcon} />
            <Typography variant="h5" className={css.titleText}>
              Shop Name
            </Typography>
          </Grid>
          <Grid container item className={css.cellTitle}>
            <RoomIcon className={css.titleIcon} />
            <Typography variant="h5" className={css.titleText}>
              Address
            </Typography>
          </Grid>
          <Grid
            container
            item
            className={css.cellTitle}
            style={{ width: "15%" }}
          >
            <LocalCarWashIcon className={css.titleIcon} />
            <Typography variant="h5" className={css.titleText}>
              Washing Units
            </Typography>
          </Grid>
          <Grid
            container
            item
            className={css.cellTitle}
            style={{ width: "15%" }}
          >
            <AccessTimeIcon className={css.titleIcon} />
            <Typography variant="h5" className={css.titleText}>
              Working Hours
            </Typography>
          </Grid>
          <Grid
            container
            item
            className={css.cellTitle}
            style={{ width: "15%" }}
          >
            <MonetizationOnOutlinedIcon className={css.titleIcon} />
            <Typography variant="h5" className={css.titleText}>
              Revenue
            </Typography>
          </Grid>
          <Grid
            container
            item
            className={css.cellTitle}
            style={{ width: "15%" }}
          >
            <BuildOutlinedIcon className={css.titleIcon} />
            <Typography variant="h5" className={css.titleText}>
              Edit / Delete
            </Typography>
          </Grid>
        </Grid>
        {allShops.length > 0 && (
          <Grid container>
            {allShops.map((x, i) => (
              <Row
                key={x.id}
                shop={x}
                revenue={allRevenues.data[i]}
                setPromptModal={props.setPromptModal}
                reloadPage={props.reloadPage}
                setInfoModalParams={props.setInfoModalParams}
                setShopFormOpen={setOpen}
              />
            ))}
            <Grid
              container
              item
              justifyContent="center"
              style={{ borderTop: "2px solid dodgerblue", padding: "1em" }}
            >
              <Grid item>
                <Pagination
                  totalCountOfItems={props.totalCountOfItems}
                  pagination={props.pagination}
                  setPagination={props.setPaginations}
                />
              </Grid>
            </Grid>
          </Grid>
        )}
        {typeof allShops.value === "string" && (
          <Grid container item className={css.noContentContainer}>
            <Grid container item className={css.noContentTitle}>
              NO SHOPS FOUND
            </Grid>
          </Grid>
        )}
      </Grid>
    </>
  );
};

export default OwnerShopTable;
