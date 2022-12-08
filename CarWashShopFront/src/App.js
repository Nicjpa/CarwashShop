import { Redirect, Route, Switch } from "react-router-dom";
import { useState } from "react";

import Header from "./components/Header";
import Login from "./components/Login";
import BookingPage from "./pages/BookingPage";
import ShopPage from "./pages/ShopPage";
import ManagementPage from "./pages/ManagementPage";

import { Grid } from "@material-ui/core";

function App() {
  const token = localStorage.getItem("token");
  const role = localStorage.getItem("role");

  const [isLoggedIn, setIsLoggedIn] = useState(!!token);

  const routesByRole =
    role === "Consumer" ? (
      <Switch>
        <Route exact path="/shops">
          <ShopPage role={role} />
        </Route>
        <Route exact path="/bookings">
          <BookingPage role={role} />
        </Route>
        <Route path="*">
          <Redirect to="/shops" />
        </Route>
      </Switch>
    ) : (
      <Switch>
        <Route exact path="/shops">
          <ShopPage role={role} />
        </Route>
        <Route exact path="/bookings">
          <BookingPage role={role} />
        </Route>
        <Route exact path="/management">
          <ManagementPage />
        </Route>
        <Route path="*">
          <Redirect to="/management" />
        </Route>
      </Switch>
    );

  const routes = isLoggedIn ? (
    routesByRole
  ) : (
    <Switch>
      <Route exact path="/login">
        <Login setLogin={setIsLoggedIn} />
      </Route>
      <Route path="*">
        <Redirect to="/login" />
      </Route>
    </Switch>
  );

  return (
    <Header isLoggedIn={isLoggedIn} setLogin={setIsLoggedIn} role={role}>
      <Grid container style={{ minHeight: "100vh" }}>
        {" "}
        {routes}
      </Grid>

      <Grid
        container
        style={{
          position: "relative",
          height: "6rem",
          backgroundColor: "rgba(35,35,47,1)",
          bottom: 0,
          borderTop: "10px solid dodgerblue",
        }}
      >
        FOOTER
      </Grid>
    </Header>
  );
}

export default App;
