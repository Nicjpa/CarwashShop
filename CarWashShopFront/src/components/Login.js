import { makeStyles } from "@material-ui/core/styles";
import React, { useState, useRef } from "react";
import {
  Grid,
  TextField,
  Button,
  IconButton,
  Slide,
  Select,
  FormControl,
  InputLabel,
  Typography,
  FormHelperText,
} from "@material-ui/core";

import AccountIcon from "@material-ui/icons/AccountCircleSharp";
import Visibility from "@material-ui/icons/Visibility";
import VisibilityOff from "@material-ui/icons/VisibilityOff";
import VpnKeySharpIcon from "@material-ui/icons/VpnKeySharp";
import AccountCircleSharpIcon from "@material-ui/icons/AccountCircleSharp";
import ArrowBackSharpIcon from "@material-ui/icons/ArrowBackSharp";
import VerifiedUserIcon from "@material-ui/icons/VerifiedUser";

import Logo from "../images/CarWashLogoPNG.png";
import LoadingModal from "../UI/Modals/LoadingModal";
import InfoModal from "../UI/Modals/InfoModal";
import { HTTPRequest } from "../HTTPRequest";

const useStyles = makeStyles((theme) => ({
  background: {
    width: "100%",
    height: "100vh",
    bottom: "1px",
    justifyContent: "center",
    alignItems: "center",
  },
  container: {
    flexDirection: "column",
    color: "white",
    position: "relative",
    backgroundColor: "rgba(0,0,0,0.8)",
    padding: "50px 100px",
    border: "5px dodgerblue solid",
    borderRadius: "12px",
    alignItems: "center",
    width: "500px",
  },

  textField: {
    margin: "0",
    color: "white",
    transition: "0.2s ease-out",
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
  passEyeIcon: {
    color: "white",
    transition: "0.2s ease-out",
    "&:hover": {
      backgroundColor: "transparent",
      color: "dodgerblue",
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
    [theme.breakpoints.down("md")]: {
      fontSize: 19,
      fontWeight: 500,
      padding: "24px",
      height: "1em",
    },
  },
  register: {
    fontFamily: "Orbitron",
    fontWeight: 900,
    fontSize: 20,
    border: "5px dodgerblue solid",
    borderRadius: "40px",
    transition: "0.15s ease-in-out",
    boxShadow: "0 0 0 gold",
    width: "100%",
    height: "1em",
    padding: "25px",
    backgroundColor: "rgba(0,0,0,0.8)",
    color: "whitesmoke",
    "&:hover": {
      backgroundColor: "limegreen",
      borderColor: "limegreen",
    },
    "&:active": {
      transition: "0.05s ease-in-out",
      boxShadow: "0 0 15px white",
      fontSize: 30,
      fontWeight: 900,
      borderColor: "white",
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
}));

const Login = (props) => {
  const css = useStyles();
  const [passVisibility, setPassVisibility] = useState(false);
  const [showLoading, setShowLoading] = useState(false);
  const [isRegistration, setIsRegistration] = useState(false);
  const [infoModalParams, setInfoModalParams] = useState({
    bool: false,
    modalTitle: "",
    modalDesc: [],
  });

  const firstNameRef = useRef();
  const lastNameRef = useRef();
  const addressRef = useRef();
  const phoneNumRef = useRef();
  const usernameRef = useRef();
  const emailRef = useRef();
  const passwordRef = useRef();
  const profileTypeRef = useRef();
  const [errorFirstName, setErrorFirstName] = useState(false);
  const [errorLastName, setErrorLastName] = useState(false);
  const [errorAddress, setErrorAddress] = useState(false);
  const [errorPhoneNum, setErrorPhoneNum] = useState(false);
  const [errorEmail, setErrorEmail] = useState(false);
  const [errorUsername, setErrorUsername] = useState(false);
  const [errorPassword, setErrorPassword] = useState(false);
  const [errorProfileType, setErrorProfileType] = useState(false);

  const loginUsernameRef = useRef();
  const loginPasswordRef = useRef();
  const [errorLoginUsername, setErrorLoginUsername] = useState(false);
  const [errorLoginPassword, setErrorLoginPassword] = useState(false);

  const preventDefaultHandler = (event) => {
    event.preventDefault();
  };

  const toggleVisibilityHandler = () => {
    setPassVisibility(!passVisibility);
  };

  const createProfileHandler = () => {
    errorLoginUsername && setErrorLoginUsername(false);
    errorLoginPassword && setErrorLoginPassword(false);
    setIsRegistration(true);
  };

  const backToLoginHandler = () => {
    errorFirstName && setErrorFirstName(false);
    errorLastName && setErrorLastName(false);
    errorAddress && setErrorAddress(false);
    errorPhoneNum && setErrorPhoneNum(false);
    errorEmail && setErrorEmail(false);
    errorUsername && setErrorUsername(false);
    errorPassword && setErrorPassword(false);
    errorProfileType && setErrorProfileType(false);
    setIsRegistration(false);
  };

  const onFocusHandler = (paramBool, paramFnc) => {
    paramBool && paramFnc(false);
  };

  const httpReq = async (httpParams, username, modalParams) => {
    try {
      setShowLoading(true);
      const response = await HTTPRequest(httpParams);
      setShowLoading(false);
      if (httpParams.action === "Login") {
        localStorage.setItem("userName", username);
        localStorage.setItem("role", response.data.role);
        localStorage.setItem("token", response.data.token);

        props.setLogin(true);
      } else if (httpParams.action === "CreateUser") {
        const role =
          profileTypeRef.current.value === "0" ? "CONSUMER" : "OWNER";

        const creationWelcome = [
          `New ${role} profile has been successfully created!`,
          `Login with your username: ${usernameRef.current.value.toUpperCase()}`,
        ];

        const ownerFeatures = [
          "Setup your car wash shop and add washing services",
          "Track your shop income and total revenue",
          "Add coowners to your shop or become one of them",
        ];

        const consumerFeatures = [
          "Book your car wash services",
          "Track your bookings",
          "Cancel your bookings",
        ];

        setInfoModalParams(() => {
          return {
            bool: true,
            modalTitle: "New Profile Created",
            modalDesc:
              role === "OWNER"
                ? [...creationWelcome, ...ownerFeatures]
                : [...creationWelcome, ...consumerFeatures],
            themeColor: "info",
          };
        });
        setIsRegistration(false);
      }
    } catch (error) {
      let errorMessage =
        error.message === "Failed to fetch"
          ? "No server response"
          : error.message.substring(0, 6) === "split,"
          ? error.message.substring(6, error.message.length - 1).split("*")
          : error.message;

      setShowLoading(false);
      setInfoModalParams(() => {
        return {
          bool: modalParams.bool,
          modalTitle: modalParams.modalTitle,
          modalDesc: errorMessage,
          themeColor: modalParams.themeColor,
        };
      });
    }
  };

  const loginCredentialsHandler = async () => {
    const username = loginUsernameRef.current.value;
    const password = loginPasswordRef.current.value;

    const isLoginValid = username.length > 0 && password.length > 0;

    if (isLoginValid) {
      const httpParams = {
        controller: "User/",
        action: "Login",
        method: "POST",
        params: "",
        body: JSON.stringify({ userName: username, password: password }),
        headers: {
          "Content-Type": "application/json",
        },
      };
      const modelParams = {
        bool: true,
        modalTitle: "Login failed",
        themeColor: "error",
      };
      await httpReq(httpParams, username, modelParams);
    } else {
      username.length === 0 && setErrorLoginUsername(true);
      password.length === 0 && setErrorLoginPassword(true);
    }
  };

  const createNewProfileHandler = async () => {
    const firstName = firstNameRef.current.value;
    const lastName = lastNameRef.current.value;
    const address = addressRef.current.value;
    const phoneNum = phoneNumRef.current.value;
    const email = emailRef.current.value;
    const userName = usernameRef.current.value;
    const password = passwordRef.current.value;
    const profileType = profileTypeRef.current.value;

    const signupIsValid =
      firstName.trim().length > 0 &&
      lastName.trim().length > 0 &&
      address.trim().length > 0 &&
      email.trim().length > 10 &&
      phoneNum.trim().length > 8 &&
      userName.trim().length > 4 &&
      password.trim().length > 4 &&
      profileType < 2;

    if (signupIsValid) {
      const httpParams = {
        controller: "User/",
        action: "CreateUser",
        method: "POST",
        params: "",
        body: JSON.stringify({
          firstName: firstName,
          lastName: lastName,
          address: address,
          phoneNumber: phoneNum,
          email: email,
          userName: userName,
          password: password,
          role: profileType,
        }),
        headers: {
          "Content-Type": "application/json",
        },
      };
      const modelParams = {
        bool: true,
        modalTitle: "Creation Failed",
        themeColor: "error",
      };
      await httpReq(httpParams, userName, modelParams);
    } else {
      firstName.trim().length === 0 && setErrorFirstName(true);
      lastName.trim().length === 0 && setErrorLastName(true);
      address.trim().length === 0 && setErrorAddress(true);
      email.trim().length < 11 && setErrorEmail(true);
      phoneNum.trim().length < 9 && setErrorPhoneNum(true);
      userName.trim().length < 5 && setErrorUsername(true);
      password.trim().length < 5 && setErrorPassword(true);
      profileType > 1 && setErrorProfileType(true);
    }
  };

  const regTextFields = [
    {
      id: "firstName",
      label: "First Name",
      ref: firstNameRef,
      type: "text",
      errorBool: errorFirstName,
      errorFnc: setErrorFirstName,
      errorMessage: "",
      maxLength: 50,
    },
    {
      id: "lastName",
      label: "Last Name",
      ref: lastNameRef,
      type: "text",
      errorBool: errorLastName,
      errorFnc: setErrorLastName,
      errorMessage: "",
      maxLength: 50,
    },
    {
      id: "address",
      label: "Address",
      ref: addressRef,
      type: "text",
      errorBool: errorAddress,
      errorFnc: setErrorAddress,
      errorMessage: "",
      maxLength: 100,
    },
    {
      id: "phoneNumber",
      label: "Phone Number",
      ref: phoneNumRef,
      type: "text",
      errorBool: errorPhoneNum,
      errorFnc: setErrorPhoneNum,
      errorMessage: "- 9 CHARS MIN",
      maxLength: 20,
    },
    {
      id: "email",
      label: "E-mail",
      ref: emailRef,
      type: "text",
      errorBool: errorEmail,
      errorFnc: setErrorEmail,
      errorMessage: "- 11 CHARS MIN",
      maxLength: 100,
    },
    {
      id: "userName",
      label: "Username",
      ref: usernameRef,
      type: "text",
      errorBool: errorUsername,
      errorFnc: setErrorUsername,
      errorMessage: "- 5 CHARS MIN",
      maxLength: 20,
    },
    {
      id: "password",
      label: "Password",
      ref: passwordRef,
      type: "password",
      errorBool: errorPassword,
      errorFnc: setErrorPassword,
      errorMessage: "- 5 CHARS MIN",
      maxLength: 20,
    },
  ];

  const selectOptions = [
    { key: "None", value: "99" },
    { key: "Consumer", value: "0" },
    { key: "Owner", value: "1" },
  ];

  return (
    <Grid container className={css.background}>
      <InfoModal params={infoModalParams} setModalBool={setInfoModalParams} />
      {showLoading && <LoadingModal loading={showLoading} />}
      {isRegistration ? (
        <Grid item>
          <Slide direction="right" in={isRegistration} timeout={400}>
            <Grid
              container
              item
              className={css.container}
              style={{
                width: "900px",
                padding: "50px",
              }}
            >
              <Button
                variant="contained"
                disableRipple
                className={css.backBtn}
                onClick={backToLoginHandler}
              >
                <ArrowBackSharpIcon id="backArrow" />
              </Button>
              <Grid
                container
                item
                justifyContent="space-evenly"
                alignItems="center"
                style={{ margin: "1em 0" }}
              >
                <Typography
                  style={{
                    fontSize: 50,
                    fontFamily: "Orbitron",
                    fontWeight: 900,
                    color: "white",
                    textShadow: "0 0 20px dodgerblue",
                  }}
                >
                  CREATE PROFILE
                </Typography>
                <AccountCircleSharpIcon
                  style={{
                    fontSize: 120,
                    color: "white",
                    filter: "drop-shadow(0 0 10px dodgerblue)",
                  }}
                />
              </Grid>
              <Grid container justifyContent="center">
                {regTextFields.map((x) => (
                  <Grid item key={x.id} style={{ margin: " 0 1em" }}>
                    <TextField
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
                <Grid item style={{ margin: "0 1em" }}>
                  <FormControl variant="filled">
                    <InputLabel className={css.categoryType}>
                      Profile Type
                    </InputLabel>
                    <Select
                      color="secondary"
                      native
                      inputProps={{
                        classes: {
                          icon: css.arrowIcon,
                        },
                      }}
                      inputRef={profileTypeRef}
                      className={css.profileType}
                      style={{
                        backgroundColor: "rgba(0,100,255, 0.1)",
                      }}
                      error={errorProfileType}
                      onFocus={() => {
                        onFocusHandler(errorProfileType, setErrorProfileType);
                      }}
                    >
                      {selectOptions.map((x) => (
                        <option
                          key={x.key}
                          value={x.value}
                          style={{
                            backgroundColor: "whitesmoke",
                            color: "dodgerblue",
                            fontWeight: 500,
                          }}
                        >
                          {x.key}
                        </option>
                      ))}
                    </Select>
                    {errorProfileType && (
                      <FormHelperText className={css.selectErrorText}>
                        * Required - Owner / Consumer
                      </FormHelperText>
                    )}
                  </FormControl>
                </Grid>
              </Grid>

              <Grid item style={{ marginTop: "10px" }}>
                <Button
                  variant="contained"
                  disableRipple
                  className={css.button}
                  style={{
                    marginTop: "0.5em",
                    width: "220px",
                  }}
                  onClick={createNewProfileHandler}
                >
                  SUBMIT
                  <VerifiedUserIcon style={{ marginLeft: "4px" }} />
                </Button>
              </Grid>
            </Grid>
          </Slide>
        </Grid>
      ) : (
        <Slide direction="down" in={!isRegistration} timeout={400}>
          <Grid item>
            <Grid container item className={css.container}>
              <Grid item>
                <img src={Logo} alt="Logo" width="200px" />
              </Grid>
              <Grid item style={{ marginTop: "40px" }}>
                <Grid container direction="column">
                  <TextField
                    error={errorLoginUsername}
                    helperText=" * Enter your username"
                    autoComplete="off"
                    autoCorrect="off"
                    color="secondary"
                    className={css.textField}
                    id="userName"
                    label="Username"
                    variant="outlined"
                    inputRef={loginUsernameRef}
                    InputProps={{
                      endAdornment: (
                        <AccountIcon
                          id="accIcon"
                          style={{
                            color: `${errorLoginUsername ? "red" : "white"}`,
                          }}
                        />
                      ),
                    }}
                    onFocus={() => {
                      onFocusHandler(errorLoginUsername, setErrorLoginUsername);
                    }}
                    onKeyDown={(event) => {
                      if (event.key === "Enter") {
                        loginCredentialsHandler();
                      }
                    }}
                  />
                  <TextField
                    error={errorLoginPassword}
                    helperText=" * Enter your password"
                    autoComplete="off"
                    autoCorrect="off"
                    color="secondary"
                    className={css.textField}
                    id="password"
                    type={passVisibility ? "text" : "password"}
                    label="Password"
                    variant="outlined"
                    inputRef={loginPasswordRef}
                    InputProps={{
                      endAdornment: (
                        <IconButton
                          onClick={toggleVisibilityHandler}
                          onMouseDown={preventDefaultHandler}
                          edge="end"
                          className={css.passEyeIcon}
                          disableRipple
                          focusRipple={false}
                        >
                          {passVisibility ? (
                            <Visibility
                              style={{
                                color: `${
                                  errorLoginPassword ? "red" : "white"
                                }`,
                              }}
                            />
                          ) : (
                            <VisibilityOff
                              style={{
                                color: `${
                                  errorLoginPassword ? "red" : "white"
                                }`,
                              }}
                            />
                          )}
                        </IconButton>
                      ),
                    }}
                    onFocus={() => {
                      onFocusHandler(errorLoginPassword, setErrorLoginPassword);
                    }}
                    onKeyDown={(event) => {
                      if (event.key === "Enter") {
                        loginCredentialsHandler();
                      }
                    }}
                  />
                </Grid>
              </Grid>
              <Grid item style={{ marginTop: "20px" }}>
                <Button
                  variant="contained"
                  disableRipple
                  className={css.button}
                  endIcon={<VpnKeySharpIcon style={{ fontSize: 28 }} />}
                  onClick={loginCredentialsHandler}
                >
                  LOG IN
                </Button>
              </Grid>
            </Grid>
            <Grid item style={{ marginTop: "10px" }}>
              <Button
                variant="contained"
                disableRipple
                className={css.register}
                onClick={createProfileHandler}
              >
                CREATE PROFILE
              </Button>
            </Grid>
          </Grid>
        </Slide>
      )}
    </Grid>
  );
};

export default Login;
