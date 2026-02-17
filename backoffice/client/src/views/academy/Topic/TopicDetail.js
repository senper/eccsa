/* eslint-disable react/prop-types */
import React from "react";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridContainer from "components/vender/Grid/GridContainer.js";
import GridItem from "components/vender/Grid/GridItem.js";
import Card from "components/vender/Card/Card.js";
import CardBody from "components/vender/Card/CardBody.js";
import CardIcon from "components/vender/Card/CardIcon.js";
import CardHeader from "components/vender/Card/CardHeader.js";
import { Check, Edit, Info } from "@material-ui/icons";
import { Link, useLocation } from "react-router-dom";
import Button from "components/vender/CustomButtons/Button.js";

import styles from "assets/jss/material-dashboard-pro-react/views/regularFormsStyle";
import { TOPIC_DETAIL_QUERY } from "variables/academy/topics_gql";
import { useQuery } from "urql";
import NavPills from "components/vender/NavPills/NavPills";
import { renderCellExpand } from "components/common/MaterialUIDataGrid/renderCellExpand";
import btnstyles from "assets/jss/material-dashboard-pro-react/views/buttonsStyle.js";
import CustomDataGrid from "components/common/MaterialUIDataGrid/CustomDataGrid";
import EditTopicModal from "./EditTopicModal";
import { statuses } from "variables/lookups";

const useStyles = makeStyles(styles);
const useBtnStyles = makeStyles(btnstyles);

export default function TopicDetail({ setMessageProps }) {
  const location = useLocation();
  const btnClasses = useBtnStyles();
  const [topic, setTopic] = React.useState(null);
  const [topicId, setTopicId] = React.useState("");
  const [result, executeQuery] = useQuery({
    query: TOPIC_DETAIL_QUERY,
    variables: { id: topicId },
    pause: true,
  });
  React.useEffect(() => {
    if (result.data && result.data.topicById) {
      const topicData = result.data.topicById;
      setTopic(topicData);
      //console.log(topic);
    } else if (result.error) {
      //console.log(result.error);
      setMessageProps({
        color: "danger",
        open: true,
        message: result.error?.message,
      });
    }
  }, [result.data, result.error]);

  React.useEffect(() => {
    if (location.state) {
      setTopicId(location.state.id);
      executeQuery();
    }
  }, [location, executeQuery]);

  const [openTopicEdit, setOpenTopicEdit] = React.useState(false);

  const resourcesColumns = [
    {
      headerName: "Name Eng",
      field: "nameEng",
      renderCell: renderCellExpand,
      flex: 0.1,
    },
    {
      headerName: "Name Local",
      field: "nameLocal",
      flex: 0.1,
    },
    {
      headerName: "Resource Type",
      field: "resourceType",
      flex: 0.08,
    },
    {
      headerName: "ResourceContent",
      field: "resourceContent",
      flex: 0.08,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.04,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          {/* use this button to add a like kind of action */}
          <Button
            justIcon
            round
            color="primary"
            size="sm"
            className={btnClasses.marginRight}
          >
            <Edit className={btnClasses.icons} />
          </Button>
        </div>
      ),
    },
  ];
  const settingsColumns = [
    {
      headerName: "Name",
      field: "settingVariable.name",
      renderCell: renderCellExpand,
      valueGetter: (params) => {
        return `${params.row.settingVariable?.nameEng} (${params.row.settingVariable?.nameLocal})`;
      },
      flex: 0.16,
    },
    {
      headerName: "Data Type",
      field: "settingVariable",
      flex: 0.08,
      valueGetter: (params) => {
        return params.value?.dataType;
      },
    },
    {
      headerName: "Value",
      field: "settingValue",
      flex: 0.08,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.04,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          {/* use this button to add a like kind of action */}
          <Button
            justIcon
            round
            color="primary"
            size="sm"
            className={btnClasses.marginRight}
          >
            <Edit className={btnClasses.icons} />
          </Button>
        </div>
      ),
    },
  ];
  const trainingsColumns = [
    {
      headerName: "Start Date",
      field: "startDate",
      flex: 0.08,
      valueGetter: (params) => {
        var dateNow = new Date(params.value);
        var month = ("0" + dateNow.getMonth()).slice(-2);
        return `${dateNow.getFullYear()}-${month}-${dateNow.getDate()}`;
      },
    },
    {
      headerName: "End Date",
      field: "endDate",
      flex: 0.08,
      valueGetter: (params) => {
        var dateNow = new Date(params.value);
        var month = ("0" + dateNow.getMonth()).slice(-2);
        return `${dateNow.getFullYear()}-${month}-${dateNow.getDate()}`;
      },
    },
    {
      headerName: "Instructor",
      field: "instructor",
      flex: 0.12,
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
            color="primary"
            size="sm"
            className={btnClasses.marginRight}
          >
            <Edit className={btnClasses.icons} />
          </Button>
        </div>
      ),
    },
  ];
  return (
    <GridContainer>
      <GridItem xs={12}>
        <Card>
          <CardHeader color="primary" icon>
            <CardIcon color="primary">
              <strong>
                {topic ? `${topic.nameEng} (${topic.nameLocal}) ` : null}
              </strong>
              {"Topic Detail"}
            </CardIcon>
            {topic ? (
              <>
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
                    setOpenTopicEdit(true);
                  }}
                >
                  <Edit className={btnClasses.icons} />
                </Button>

                <EditTopicModal
                  setMessageProps={setMessageProps}
                  openModal={openTopicEdit}
                  setOpenModal={setOpenTopicEdit}
                  topic={topic}
                  setTopic={setTopic}
                  reloadParentData={executeQuery}
                />
              </>
            ) : null}

            <Link
              style={{ color: "#a32705", fontWeight: "1000" }}
              to="/academy/topics"
            >
              {"<< back to list"}
            </Link>
          </CardHeader>
          <CardBody>
            {topic ? (
              <GridContainer>
                <GridItem xs={12} sm={12} md={12}>
                  Code: <strong>{topic.code}</strong>
                  <br />
                  Name:{" "}
                  <strong>
                    {topic.nameEng} ({topic.nameLocal})
                  </strong>
                  <br />
                  Description:{" "}
                  <strong>
                    {topic.decription} ({topic.decriptionLocal})
                  </strong>
                  <br />
                  Category:{" "}
                  <strong>
                    {topic.category.code} {topic.category.nameEng} (
                    {topic.category.nameLocal})
                  </strong>
                  <br />
                  Status:{" "}
                  <strong>
                    {statuses.find((item) => item.id == topic.status).name}
                  </strong>
                  <br />
                </GridItem>
              </GridContainer>
            ) : null}
            <hr style={{ marginBottom: 0 }} />
            {topic ? (
              <GridContainer>
                <GridItem xs={12} sm={12} md={12}>
                  <NavPills
                    color="primary"
                    tabs={[
                      {
                        tabButton: "Resources",
                        tabContent: (
                          <CustomDataGrid
                            dataRows={topic.resources}
                            columns={resourcesColumns}
                          />
                        ),
                      },
                      {
                        tabButton: "Settings",
                        tabContent: (
                          <CustomDataGrid
                            dataRows={topic.topicSettings}
                            columns={settingsColumns}
                          />
                        ),
                      },
                      {
                        tabButton: "Training",
                        tabContent: (
                          <CustomDataGrid
                            dataRows={topic.trainings}
                            columns={trainingsColumns}
                          />
                        ),
                      },
                    ]}
                  />
                </GridItem>
              </GridContainer>
            ) : null}
          </CardBody>
        </Card>
      </GridItem>
    </GridContainer>
  );
}
