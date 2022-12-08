import ControlPanel from "../components/ControlPanel";
import { Grid } from "@material-ui/core";

const ManagementPage = () => {
  return (
    <Grid container justifyContent="center">
      <Grid container style={{ padding: "1rem 20rem", width: "1920px" }}>
        <ControlPanel />
      </Grid>
    </Grid>
  );
};

export default ManagementPage;
