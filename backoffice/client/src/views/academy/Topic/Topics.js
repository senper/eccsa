/* eslint-disable react/prop-types */
import React from "react";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridContainer from "components/vender/Grid/GridContainer.js";
import GridItem from "components/vender/Grid/GridItem.js";
import Button from "components/vender/CustomButtons/Button.js";
import Card from "components/vender/Card/Card.js";
import CardBody from "components/vender/Card/CardBody.js";
import CardIcon from "components/vender/Card/CardIcon.js";
import CardHeader from "components/vender/Card/CardHeader.js";

import { cardTitle } from "assets/jss/material-dashboard-pro-react.js";
import Info from "@material-ui/icons/Info";
import { Link } from "react-router-dom";
import { TOPICS_QUERY } from "variables/academy/topics_gql";
import { useQuery } from "urql";
import CustomDataGrid from "components/common/MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "components/common/MaterialUIDataGrid/renderCellExpand";
import btnstyles from "assets/jss/material-dashboard-pro-react/views/buttonsStyle.js";
import { Add } from "@material-ui/icons";
import EditTopicModal from "./EditTopicModal";

const styles = {
  cardIconTitle: {
    ...cardTitle,
    marginTop: "15px",
    marginBottom: "0px",
  },
};

const useBtnStyles = makeStyles(btnstyles);
const useStyles = makeStyles(styles);

export default function Topics({ setMessageProps }) {
  const [topics, setTopics] = React.useState([]);
  const [result, executeQuery] = useQuery({
    query: TOPICS_QUERY,
  });
  React.useEffect(() => {
    if (result.data && result.data.topics) {
      const topicsData = result.data.topics;
      setTopics(topicsData);
      //console.log(topicsData);
    } else if (result.error) {
      //console.log(result.error);
      setMessageProps({
        color: "danger",
        open: true,
        message: result.error?.message,
      });
    }
  }, [result.data, result.error]);

  const [openTopicAdd, setOpenTopicAdd] = React.useState(false);

  const columns = [
    {
      headerName: "Name",
      field: "nameEng",
      renderCell: renderCellExpand,
      flex: 0.3,
    },
    {
      headerName: "Category",
      field: "category",
      flex: 0.08,
      valueGetter: (params) => {
        return params.value.nameEng;
      },
    },
    {
      headerName: "Description",
      field: "decription",
      flex: 0.08,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.08,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          {/* use this button to add a like kind of action */}
          <Button
            justIcon
            round
            color="info"
            size="sm"
            className={btnClasses.marginRight}
          >
            <Link
              style={{ color: "white" }}
              to={{
                pathname: "/academy/topic_detail",
                state: { id: params.value },
              }}
            >
              <Info className={btnClasses.icons} />
            </Link>
          </Button>
        </div>
      ),
    },
  ];
  const classes = useStyles();
  const btnClasses = useBtnStyles();
  return (
    <>
      <GridContainer>
        <GridItem xs={12}>
          <Card>
            <CardHeader color="primary" icon>
              <CardIcon color="primary">{"Topics"}</CardIcon>
              <Button
                style={{
                  position: "absolute",
                  right: "-10px",
                }}
                justIcon
                round
                color="primary"
                size="sm"
                className={btnClasses.marginRight}
                onClick={(event) => {
                  setOpenTopicAdd(true);
                }}
              >
                <Add className={btnClasses.icons} />
              </Button>

              {openTopicAdd ? (
                <EditTopicModal
                  setMessageProps={setMessageProps}
                  openModal={openTopicAdd}
                  setOpenModal={setOpenTopicAdd}
                  topic={{ categoryId: "", status: "" }}
                  reloadParentData={executeQuery}
                />
              ) : null}
            </CardHeader>
            <CardBody>
              <CustomDataGrid dataRows={topics} columns={columns} />
            </CardBody>
          </Card>
        </GridItem>
      </GridContainer>
    </>
  );
}
