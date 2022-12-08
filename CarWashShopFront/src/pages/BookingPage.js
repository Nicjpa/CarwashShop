import { makeStyles } from "@material-ui/core/styles";

import { useState, useEffect, useRef } from "react";
import BookingFilter from "../components/Filters/BookingFilter";
import BookingCard from "../UI/Cards/BookingCard";
import Pagination from "../components/Pagination";
import { HTTPRequest } from "../HTTPRequest";
import { Grid, Zoom } from "@material-ui/core";
import LoadingModal from "../UI/Modals/LoadingModal";
import PromptModal from "../UI/Modals/PromptModal";
import InfoModal from "../UI/Modals/InfoModal";

const useStyles = makeStyles((theme) => ({
  filterPanelGrid: {
    backgroundColor: "rgba(0,0,50,0.55)",
    border: "7px solid white",
    padding: "2em",
    alignItems: "center",
    borderRadius: "24px",
    backdropFilter: "blur(10px)",
    justifyContent: "space-around",
  },
  noContentContainer: {
    padding: "5rem",
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
  noContentSubtitle: {
    paddingTop: "0.4rem",
    width: "auto",
    color: "white",
    fontFamily: "Orbitron",
    fontSize: 20,
    fontWeight: 700,
    textShadow: "0 0 3px dodgerblue",
    letterSpacing: "0.5em",
    textAlign: "center",
  },
}));

const BookingPage = (props) => {
  const css = useStyles();
  const role = props.role;
  const [showLoading, setShowLoading] = useState(false);
  const [filterParams, setFilterParams] = useState(
    "&MinPrice=1&MaxPrice=100&ScheduledHoursAfter=1&ScheduledHoursBefore=24"
  );
  const [reloadPage, setReloadPage] = useState(false);

  const reloadPageHandler = () => {
    setReloadPage(!reloadPage);
  };

  const [allBookings, setAllBookings] = useState({
    data: [],
    numOfPages: 0,
    totalCountOfItems: 0,
  });
  const [pagination, setPaginations] = useState({
    currentPage: 1,
    recordsPerPage: 10,
  });

  const [promptModal, setPromptModal] = useState({
    bool: false,
    title: "Cancel booking?",
    body: null,
    id: null,
  });

  const [infoModalParams, setInfoModalParams] = useState({
    bool: false,
    modalTitle: "",
    modalDesc: [],
  });

  useEffect(() => {
    const consumerHttpParams = {
      controller: "ConsumerManagement/",
      action: `GetAllBookings?Page=${pagination.currentPage}&RecordsPerPage=${pagination.recordsPerPage}`,
      method: "GET",
      params: filterParams,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const ownerHttpParams = {
      controller: "OwnerManagement/",
      action: `GetShopBookings?Page=${pagination.currentPage}&RecordsPerPage=${pagination.recordsPerPage}`,
      method: "GET",
      params: filterParams,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const httpParams =
      role === "Consumer" ? consumerHttpParams : ownerHttpParams;

    const httpCall = async () => {
      try {
        setShowLoading(true);
        const response = await HTTPRequest(httpParams);
        setAllBookings(() => {
          return response;
        });
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
            : "Failed to delete";

        setInfoModalParams(() => {
          return {
            bool: true,
            modalTitle: modalTitle,
            modalDesc: errorMessage,
            themeColor: "error",
          };
        });
      }

      setTimeout(() => {
        setShowLoading(false);
      }, 400);
    };

    httpCall();
  }, [filterParams, pagination, reloadPage, role]);

  const deleteBookingHandle = async () => {
    const httpParams = {
      controller: "ConsumerManagement/",
      action: "cancelBookingById",
      method: "DELETE",
      params: `?bookingID=${promptModal.id}`,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    try {
      setPromptModal((prevValue) => {
        return { ...prevValue, bool: false };
      });

      setShowLoading(true);

      await HTTPRequest(httpParams);

      setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: "Booking canceled",
          modalDesc: `Booking #${promptModal.id} is successfully canceled.`,
          themeColor: "info",
        };
      });
      setReloadPage(!reloadPage);
    } catch (error) {
      let errorMessage =
        error.message === "Failed to fetch"
          ? "No server response"
          : error.message.substring(0, 6) === "split,"
          ? error.message.substring(6, error.message.length - 1).split("*")
          : error.message;

      setPromptModal((prevValue) => {
        return { ...prevValue, bool: false };
      });

      setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: "Failed to cancel",
          modalDesc: errorMessage,
          themeColor: "error",
        };
      });
    }
    setShowLoading(false);
  };

  return (
    <Grid container justifyContent="center">
      <LoadingModal loading={showLoading} />

      {!promptModal.bool && !showLoading && (
        <InfoModal params={infoModalParams} setModalBool={setInfoModalParams} />
      )}

      {!infoModalParams.bool && !showLoading && (
        <PromptModal
          promptModal={promptModal}
          closeModal={setPromptModal}
          executeYes={deleteBookingHandle}
        />
      )}

      <Grid container style={{ padding: "0 6em 3.5em 6em", width: "1920px" }}>
        <Zoom
          in={true}
          timeout={500}
          style={{
            transitionDelay: "400ms",
          }}
        >
          <Grid container item>
            <BookingFilter setFilterParams={setFilterParams} role={role} />
          </Grid>
        </Zoom>

        <Zoom in={true} timeout={500} style={{ transitionDelay: "50ms" }}>
          <Grid container>
            <Grid
              container
              justifyContent="center"
              style={{
                margin: "1em 0",
                padding: "1em",
                borderRadius: "24px",
              }}
            >
              {allBookings.data.length > 0 && (
                <>
                  <Grid container item justifyContent="center">
                    {allBookings.data.map((x) => (
                      <Grid key={x.id} item lg={6} md={12}>
                        <BookingCard
                          key={x.id}
                          booking={x}
                          cancelModal={setPromptModal}
                          setInfoModalParams={setInfoModalParams}
                          reloadPage={reloadPageHandler}
                          role={role}
                        />
                      </Grid>
                    ))}
                  </Grid>
                  <Grid
                    container
                    item
                    style={{
                      width: "auto",
                      backgroundColor: "rgba(0,0,50,0.55)",
                      borderRadius: "25px",
                      marginTop: "1rem",
                    }}
                  >
                    <Pagination
                      totalCountOfItems={allBookings.totalCountOfItems}
                      pagination={pagination}
                      setPagination={setPaginations}
                    />
                  </Grid>
                </>
              )}
              {typeof allBookings.data.value === "string" && (
                <Grid container className={css.filterPanelGrid}>
                  <Grid container item className={css.noContentContainer}>
                    <Grid container item className={css.noContentTitle}>
                      NO BOOKING FOUND
                    </Grid>
                  </Grid>
                </Grid>
              )}
            </Grid>
          </Grid>
        </Zoom>
        {/* )} */}
      </Grid>
    </Grid>
  );
};

export default BookingPage;
