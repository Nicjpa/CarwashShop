import { makeStyles } from "@material-ui/core/styles";
import { useState, useRef, useEffect } from "react";
import { HTTPRequest } from "../../HTTPRequest";
import {
  Grid,
  Typography,
  Button,
  Slide,
  TextField,
  Slider,
  Backdrop,
  Modal,
} from "@material-ui/core";

import MonetizationOnIcon from "@material-ui/icons/MonetizationOn";
import ServiceIcon from "@material-ui/icons/LocalCarWash";
import WatchIcon from "@material-ui/icons/AccessTimeSharp";
import StorefrontIcon from "@material-ui/icons/Storefront";
import SaveSharpIcon from "@material-ui/icons/SaveSharp";
import AddCircleOutlineSharpIcon from "@material-ui/icons/AddCircleOutlineSharp";

const useStyles = makeStyles((theme) => ({
  container: {
    backgroundColor: "rgba(0,0,0, 0.5)",
    flexDirection: "column",
    color: "white",
    position: "relative",
    backdropFilter: "blur(20px)",
    padding: "50px 100px",
    border: "5px white solid",
    borderRadius: "12px",
    alignItems: "center",
    width: "500px",
    outline: "none",
    boxShadow: "0 0 15px dodgerblue",
    "&:focus": {
      outline: "none",
    },
  },

  regsterField: {
    color: "white",
    transition: "0.2s ease-out",
    borderRadius: "4px",
    "& .MuiFilledInput-root": {
      backgroundColor: "rgba(0,100,255, 0.1)",
      borderRadiusColor: "4px",
    },
    "& .MuiFilledInput-root.Mui-focused": {
      backgroundColor: "rgba(0,100,255, 0.2)",
      borderRadiusColor: "4px",
    },
    "& .MuiFormHelperText-root.Mui-error": {
      color: "red",
      fontFamily: "Orbitron",
      fontSize: 12,
      userSelect: "none",
    },
    "& .MuiFormHelperText-root": {
      color: "transparent",
      margin: 0,
      marginBottom: "10px",
      fontSize: 12,
      userSelect: "none",
    },
  },

  button: {
    fontFamily: "Orbitron",
    fontWeight: 900,
    fontSize: 20,
    border: "3px dodgerblue solid",
    borderRadius: "40px",
    transition: "0.15s ease-in-out",
    boxShadow: "0 0 0 gold",
    height: "1em",
    padding: "25px",
    backgroundColor: "dodgerblue",
    color: "white",
    "&:hover": {
      backgroundColor: "transparent",
      color: "white",
    },
    "&:active": {
      transition: "none",
      boxShadow: "0 0 15px dodgerblue",
      color: "dodgerblue",
    },
  },

  categoryType: {
    fontSize: 20,
    top: "-2px",
    fontWeight: 700,
    fontFamily: "Orbitron",
    transition: "none",
    color: "white",

    "&:focus": {
      color: "dodgerblue",
    },
    "&.MuiInputLabel-root.Mui-focused": {
      color: "dodgerblue",
    },
  },
  arrowIcon: {
    fill: "white",
  },

  profileType: {
    width: "320px",
    backgroundColor: "rgba(0,100,255, 0.1)",
    "&:hover": {
      backgroundColor: "rgba(0,100,255, 0.2)",
    },
    "&:focus": {
      backgroundColor: "rgba(0,100,255, 0.2)",
    },
  },

  backBtn: {
    marginTop: "0",
    position: "absolute",
    top: "1em",
    left: "1em",
    backgroundColor: "transparent",
    color: "white",
    borderRadius: "360px",
    padding: "0.2em",
    border: "4px transparent solid",
    boxShadow: "0 0 15px transparent",
    transition: "0.15s ease-out",
    "& #backArrow": {
      fontSize: 50,
      transition: "0.15s ease-out",
    },
    "&:hover #backArrow": {
      color: "dodgerblue",
    },
    "&:hover": {
      backgroundColor: "transparent",
      boxShadow: "0 0 15px transparent",
    },
    "&:active": {
      border: "4px white solid",
      boxShadow: "0 0 15px dodgerblue",
    },
  },
  selectErrorText: {
    color: "red",
    fontFamily: "Orbitron",
    fontSize: 12,
    margin: 0,
    userSelect: "none",
  },
  backdrop: {
    display: "flex",
    zIndex: 2000,
    outline: "none",
    justifyContent: "center",
    alignItems: "center",
    "& .MuiBackdrop-root": {
      backgroundColor: "rgba(0,0,0,0.5)",
      backdropFilter: "blur(5px)",
      outline: "1px green solid",
      outline: "1px green solid",
    },
  },
  valueLabel: {
    color: "transparent",
    fontFamily: "Orbitron",
    fontWeight: 100,
    textShadow: "0 0 8px dodgerblue",
    top: "-20px",
    "& > span > span": {
      color: "white",
      fontWeight: 600,
      position: "relative",
      right: "27px",
      top: "35px",
    },
  },
  priceRangeLabel: {
    color: "White",
    fontFamily: "Orbitron",
    fontWeight: 500,
    display: "flex",
    alignItems: "center",
    marginBottom: "0.8em",
  },
  priceGrid: {
    width: "15.8rem",
    height: "auto",
    border: "2px dodgerblue solid",
    borderRadius: "12px",
    padding: "1rem 0",
    transition: "0.15s linear",
    marginBottom: "1.5rem",
  },
}));

const ShopModal = (props) => {
  const css = useStyles();
  const shopNameRef = useRef();
  const addressRef = useRef();
  const [errorShopName, setErrorShopName] = useState(false);
  const [errorAddress, setErrorAddress] = useState(false);
  const [workingHours, setWorkingHours] = useState([1, 24]);
  const [washingUnits, setWashingUnits] = useState(1);

  const serviceNameRef = useRef();
  const descritpionRef = useRef();
  const [errorServiceName, setErrorServiceName] = useState(false);
  const [errorDescription, setErrorDescription] = useState(false);
  const [price, setPrice] = useState(1);

  const workingHoursHandler = (event, newValue) => {
    setWorkingHours(newValue);
  };

  const washingUnitsHandler = (event, newValue) => {
    setWashingUnits(newValue);
  };

  const priceHandler = (event, newValue) => {
    setPrice(newValue);
  };

  useEffect(() => {
    if (props.open.edit) {
      const openingTime = parseInt(props.open.shopInfo.openingTime);

      const closingTime = parseInt(props.open.shopInfo.closingTime);

      const washingUnits = parseInt(props.open.shopInfo.amountOfWashingUnits);

      setWorkingHours([openingTime, closingTime]);
      setWashingUnits(washingUnits);
    } else {
      setWorkingHours([1, 24]);
      setWashingUnits(1);
      setPrice(1);
    }
  }, [props.open.open]);

  const closeModalHandler = () => {
    errorShopName && setErrorShopName(false);
    errorAddress && setErrorAddress(false);
    errorServiceName && setErrorServiceName(false);
    errorDescription && setErrorDescription(false);

    props.setOpen((prevValue) => {
      return { ...prevValue, open: false };
    });
  };

  const onFocusHandler = (paramBool, paramFnc) => {
    paramBool && paramFnc(false);
  };

  const httpCall = async (httpParams, errorTitle, infoModalParams) => {
    try {
      await HTTPRequest(httpParams);
      props.setOpen((prevValue) => {
        return { ...prevValue, open: false };
      });
      props.reloadPage();
      props.setInfoModalParams(() => {
        return { ...infoModalParams };
      });
    } catch (error) {
      console.log(error);
      let errorMessage =
        error.message === "Failed to fetch"
          ? "No server response"
          : error.message.substring(0, 6) === "split,"
          ? error.message.substring(6, error.message.length - 1).split("*")
          : error.message;
      const modalTitle =
        errorMessage === "No server response"
          ? "Connection problem"
          : errorTitle;
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

  const createNewShopHandler = async () => {
    const shopName = shopNameRef.current.value;
    const address = addressRef.current.value;
    const serviceName = serviceNameRef.current.value;
    const description = descritpionRef.current.value;

    const signupIsValid =
      shopName.trim().length > 0 &&
      address.trim().length > 0 &&
      serviceName.trim().length > 0 &&
      description.trim().length > 0;

    if (signupIsValid) {
      const httpParams = {
        controller: "OwnerShops/",
        action: "CreateNewShop",
        method: "POST",
        params: "",
        body: JSON.stringify({
          name: shopName,
          advertisingDescription: "",
          address: address,
          amountOfWashingUnits: washingUnits,
          openingTime: workingHours[0],
          closingTime: workingHours[1],
          services: [
            {
              name: serviceName,
              description: description,
              price: price,
            },
          ],
        }),
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + localStorage.getItem("token"),
        },
      };

      const infoModalParams = {
        bool: true,
        modalTitle: "New shop created",
        modalDesc: `You have successfully created '${shopName}'.`,
        themeColor: "info",
      };

      await httpCall(httpParams, "Shop creation failed", infoModalParams);
    } else {
      shopName.trim().length === 0 && setErrorShopName(true);
      address.trim().length === 0 && setErrorAddress(true);
      serviceName.trim().length === 0 && setErrorServiceName(true);
      description.trim().length === 0 && setErrorDescription(true);
    }
  };

  const editShopHandler = async () => {
    const shopName = shopNameRef.current.value;
    const address = addressRef.current.value;

    const signupIsValid =
      shopName.trim().length > 0 && address.trim().length > 0;

    if (signupIsValid) {
      const httpParams = {
        controller: "OwnerShops/",
        action: "UpdateShopInfo",
        method: "PUT",
        params: "",
        body: JSON.stringify({
          id: props.open.shopInfo.id,
          name: shopName,
          advertisingDescription: "string",
          amountOfWashingUnits: washingUnits,
          address: address,
          openingTime: workingHours[0],
          closingTime: workingHours[1],
        }),
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + localStorage.getItem("token"),
        },
      };

      const infoModalParams = {
        bool: true,
        modalTitle: "Shop edited",
        modalDesc: `Changes has been made to '${shopName}'.`,
        themeColor: "info",
      };

      await httpCall(httpParams, "Edit shop failed", infoModalParams);
    } else {
      shopName.trim().length === 0 && setErrorShopName(true);
      address.trim().length === 0 && setErrorAddress(true);
    }
  };

  const regShopFields = [
    {
      id: "shopName",
      label: "Shop Name",
      ref: shopNameRef,
      type: "text",
      errorBool: errorShopName,
      errorFnc: setErrorShopName,
      errorMessage: "",
      maxLength: 15,
      defaultValue: props.open.edit ? props.open.shopInfo.name : "",
    },
    {
      id: "address",
      label: "Address",
      ref: addressRef,
      type: "text",
      errorBool: errorAddress,
      errorFnc: setErrorAddress,
      errorMessage: "",
      maxLength: 30,
      defaultValue: props.open.edit ? props.open.shopInfo.address : "",
    },
  ];

  const regServiceFields = [
    {
      id: "serviceName",
      label: "Service Name",
      ref: serviceNameRef,
      type: "text",
      errorBool: errorServiceName,
      errorFnc: setErrorServiceName,
      errorMessage: "",
      maxLength: 15,
    },
    {
      id: "description",
      label: "Description",
      ref: descritpionRef,
      type: "text",
      errorBool: errorDescription,
      errorFnc: setErrorDescription,
      errorMessage: "",
      maxLength: 150,
    },
  ];

  return (
    <Modal
      className={css.backdrop}
      open={props.open.open}
      BackdropComponent={Backdrop}
      onClose={closeModalHandler}
    >
      <Slide
        direction={props.open.edit ? "down" : "right"}
        in={props.open.open}
        timeout={400}
      >
        <Grid
          container
          item
          className={css.container}
          style={{
            width: "auto",
            padding: "50px",
          }}
        >
          <Grid
            container
            item
            direction={props.open.edit ? "column" : "row"}
            justifyContent={props.open.edit ? "center" : "space-evenly"}
            alignItems="center"
            style={{ margin: "-1rem 0 2em 0" }}
          >
            {props.open.edit && (
              <StorefrontIcon
                style={{
                  fontSize: 110,
                  color: "white",
                  filter: "drop-shadow(0 0 10px dodgerblue)",
                }}
              />
            )}
            <Typography
              style={{
                fontSize: 44,
                fontFamily: "Orbitron",
                fontWeight: 900,
                color: "white",
                textShadow: "0 0 20px dodgerblue",
              }}
            >
              {props.open.edit ? "EDIT SHOP" : "CREATE NEW SHOP"}
            </Typography>
            {!props.open.edit && (
              <StorefrontIcon
                style={{
                  fontSize: 110,
                  color: "white",
                  filter: "drop-shadow(0 0 10px dodgerblue)",
                  marginLeft: "1rem",
                }}
              />
            )}
          </Grid>
          <Grid container style={{ width: "auto" }}>
            <Grid
              container
              style={{ width: "auto" }}
              direction="column"
              justifyContent="flex-start"
              alignItems="center"
            >
              {regShopFields.map((x) => (
                <Grid item key={x.id} style={{ margin: " 0 1em" }}>
                  <TextField
                    key={x.id}
                    defaultValue={x.defaultValue}
                    autoComplete="off"
                    autoCorrect="off"
                    color="secondary"
                    className={css.regsterField}
                    id={x.id}
                    label={x.label}
                    type={x.type}
                    variant="filled"
                    inputRef={x.ref}
                    error={x.errorBool}
                    helperText={`* Required ${x.errorMessage}`}
                    inputProps={{ maxLength: x.maxLength }}
                    style={{ width: "320px" }}
                    onFocus={() => {
                      onFocusHandler(x.errorBool, x.errorFnc);
                    }}
                    onKeyDown={(e) => {
                      if (
                        e.keyCode === 32 &&
                        (x.id === "userName" ||
                          x.id === "password" ||
                          x.id === "email")
                      ) {
                        e.preventDefault();
                      } else if (
                        e.keyCode !== 8 &&
                        (e.keyCode < 48 || e.keyCode > 57) &&
                        (e.keyCode < 96 || e.keyCode > 105) &&
                        x.id === "phoneNumber"
                      ) {
                        e.preventDefault();
                      }
                    }}
                  />
                </Grid>
              ))}
              <Grid
                container
                item
                direction="column"
                justifyContent="center"
                alignItems="center"
                className={css.priceGrid}
                style={{
                  transition: "none",
                }}
              >
                <Grid item>
                  <Typography className={css.priceRangeLabel} gutterBottom>
                    <WatchIcon style={{ marginRight: "0.15em" }} />
                    Working Time
                  </Typography>
                  <Slider
                    style={{
                      width: "200px",
                    }}
                    classes={{ valueLabel: css.valueLabel }}
                    min={1}
                    max={24}
                    step={1}
                    value={workingHours}
                    onChange={workingHoursHandler}
                    valueLabelDisplay="on"
                  />
                </Grid>
                <Grid item style={{ marginTop: "2rem" }}>
                  <Typography className={css.priceRangeLabel} gutterBottom>
                    <ServiceIcon style={{ marginRight: "0.15em" }} />
                    Washing Units
                  </Typography>
                  <Slider
                    style={{
                      width: "200px",
                    }}
                    classes={{ valueLabel: css.valueLabel }}
                    min={1}
                    max={50}
                    step={1}
                    value={washingUnits}
                    onChange={washingUnitsHandler}
                    valueLabelDisplay="on"
                  />
                </Grid>
              </Grid>
            </Grid>
            {!props.open.edit && (
              <Grid
                container
                style={{ width: "auto" }}
                direction="column"
                alignItems="center"
              >
                {regServiceFields.map((x) => (
                  <Grid item key={x.id} style={{ margin: " 0 1em" }}>
                    <TextField
                      multiline
                      key={x.id}
                      autoComplete="off"
                      autoCorrect="off"
                      color="secondary"
                      className={css.regsterField}
                      id={x.id}
                      label={x.label}
                      type={x.type}
                      variant="filled"
                      inputRef={x.ref}
                      error={x.errorBool}
                      helperText={`* Required ${x.errorMessage}`}
                      inputProps={{ maxLength: x.maxLength }}
                      style={{ width: "320px" }}
                      onFocus={() => {
                        onFocusHandler(x.errorBool, x.errorFnc);
                      }}
                      onKeyDown={(e) => {
                        if (
                          e.keyCode === 32 &&
                          (x.id === "userName" ||
                            x.id === "password" ||
                            x.id === "email")
                        ) {
                          e.preventDefault();
                        } else if (
                          e.keyCode !== 8 &&
                          (e.keyCode < 48 || e.keyCode > 57) &&
                          (e.keyCode < 96 || e.keyCode > 105) &&
                          x.id === "phoneNumber"
                        ) {
                          e.preventDefault();
                        }
                      }}
                    />
                  </Grid>
                ))}
                <Grid
                  container
                  item
                  direction="column"
                  justifyContent="center"
                  alignItems="center"
                  className={css.priceGrid}
                  style={{
                    borderColor: "dodgerblue",
                    transition: "none",
                  }}
                >
                  <Typography className={css.priceRangeLabel} gutterBottom>
                    <MonetizationOnIcon
                      style={{ marginRight: "0.15em", marginLeft: "-0.5rem" }}
                    />
                    Price
                  </Typography>
                  <Slider
                    style={{
                      width: "200px",
                      marginBottom: "0.8rem",
                    }}
                    classes={{ valueLabel: css.valueLabel }}
                    min={1}
                    max={100}
                    step={1}
                    value={price}
                    onChange={priceHandler}
                    valueLabelDisplay="on"
                  />
                </Grid>
                <Grid item style={{ marginTop: "10px" }}>
                  <Button
                    variant="contained"
                    disableRipple
                    className={css.button}
                    style={{
                      marginTop: "0.6rem",
                      width: "220px",
                    }}
                    onClick={createNewShopHandler}
                  >
                    CREATE
                    <AddCircleOutlineSharpIcon style={{ marginLeft: "4px" }} />
                  </Button>
                </Grid>
              </Grid>
            )}
          </Grid>
          {props.open.edit && (
            <Grid item style={{ marginTop: "10px" }}>
              <Button
                variant="contained"
                disableRipple
                className={css.button}
                style={{
                  marginTop: "0.6rem",
                  width: "220px",
                }}
                onClick={editShopHandler}
              >
                SAVE
                <SaveSharpIcon style={{ marginLeft: "4px", fontSize: 26 }} />
              </Button>
            </Grid>
          )}
        </Grid>
      </Slide>
    </Modal>
  );
};

export default ShopModal;
