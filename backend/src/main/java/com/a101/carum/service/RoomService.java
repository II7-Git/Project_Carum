package com.a101.carum.service;

import com.a101.carum.api.dto.*;
import com.a101.carum.common.exception.UnAuthorizedException;
import com.a101.carum.domain.furniture.Furniture;
import com.a101.carum.domain.interior.Interior;
import com.a101.carum.domain.inventory.Inventory;
import com.a101.carum.domain.room.Room;
import com.a101.carum.domain.user.User;
import com.a101.carum.domain.user.UserDetail;
import com.a101.carum.repository.*;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

@Service
@RequiredArgsConstructor
public class RoomService {
    private final UserRepository userRepository;
    private final UserDetailRepository userDetailRepository;
    private final RoomRepository roomRepository;
    private final CustomRoomRepository customRoomRepository;
    private final InteriorRepository interiorRepository;
    private final FurnitureRepository furnitureRepository;
    private final InventoryRepository inventoryRepository;

    private final String BACKGROUND = "WHITE,BLACK";

    @Transactional
    public void createRoom(ReqPostRoom reqPostRoom, Long id) {
        User user = userRepository.findByIdAndIsDeleted(id, false)
                .orElseThrow(() -> new NullPointerException("User를 찾을 수 없습니다."));

        Room room = Room.builder()
                .name(reqPostRoom.getName())
                .user(user)
                .background(BACKGROUND)
                .emotionTag("")
                .build();

        roomRepository.save(room);
        
        //TODO: 기본 가구 배치
    }

    @Transactional
    public void updateRoom(ReqPatchRoom reqPatchRoom, Long id, Long roomId) {
        User user = userRepository.findByIdAndIsDeleted(id, false)
                .orElseThrow(() -> new NullPointerException("User를 찾을 수 없습니다."));
        Room room = roomRepository.findByIdAndUser(roomId, user)
                .orElseThrow(() -> new NullPointerException("Room을 찾을 수 없습니다."));

        if(reqPatchRoom.getName() != null) {
            room.updateName(reqPatchRoom.getName());
        }

        if(reqPatchRoom.getEmotionTags() != null) {
            StringBuilder sb = new StringBuilder();
            Collections.sort(reqPatchRoom.getEmotionTags());
            for(String tag: reqPatchRoom.getEmotionTags()){
                sb.append(tag).append(",");
            }
            room.updateEmotionTag(sb.toString());
        }
        
        //TODO: Background 처리
    }

    @Transactional
    public ResGetRoomList readRoomList(ReqGetRoomList reqGetRoomList, Long id) {
        User user = userRepository.findByIdAndIsDeleted(id, false)
                .orElseThrow(() -> new NullPointerException("User를 찾을 수 없습니다."));
        UserDetail userDetail = userDetailRepository.findByUser(user)
                .orElseThrow(() -> new NullPointerException("User 정보가 손상되었습니다."));

        ResGetRoomList.ResGetRoomListBuilder resGetRoomListBuilder = ResGetRoomList.builder();
        resGetRoomListBuilder.mainRoomId(
                userDetail.getMainRoom().getId()
        );

        List<ResGetRoom> roomList = customRoomRepository.readRoomList(user, reqGetRoomList.getTags());
        resGetRoomListBuilder.roomList(roomList);
        return resGetRoomListBuilder.build();
    }

    @Transactional
    public void updateInterior(ReqPutRoom reqPutRoom, Long id, Long roomId) {
        User user = userRepository.findByIdAndIsDeleted(id, false)
                .orElseThrow(() -> new NullPointerException("User를 찾을 수 없습니다."));
        Room room = roomRepository.findByIdAndUser(roomId, user)
                .orElseThrow(() -> new NullPointerException("Room을 찾을 수 없습니다."));

        if(reqPutRoom.getBackground() != null) {
            StringBuilder sb = new StringBuilder();
            for(String color: reqPutRoom.getBackground()){
                sb.append(color).append(",");
            }
            room.updateBackground(sb.toString());
        }

        if(reqPutRoom.getInteriors() != null) {
            for(ReqPutRoomDetail reqPutRoomDetail: reqPutRoom.getInteriors()){
                switch (reqPutRoomDetail.getAction()){
                    case ADD:
                        Furniture furniture = furnitureRepository.findById(reqPutRoomDetail.getFurnitureId())
                                .orElseThrow(() -> new NullPointerException("Furniture를 찾을 수 없습니다."));
                        inventoryRepository.findByUserAndFurniture(user, furniture)
                                .orElseThrow(() -> new UnAuthorizedException("가구를 구매한적 없습니다"));
                        interiorRepository.save(Interior.builder()
                                .room(room)
                                .furniture(furniture)
                                .x(reqPutRoomDetail.getX())
                                .y(reqPutRoomDetail.getY())
                                .z(reqPutRoomDetail.getZ())
                                .xRot(reqPutRoomDetail.getRotX())
                                .yRot(reqPutRoomDetail.getRotY())
                                .zRot(reqPutRoomDetail.getRotZ())
                                .build()
                        );
                        break;
                    case DEL:
                        Interior interiorDelete = interiorRepository.findById(reqPutRoomDetail.getInteriorId())
                                .orElseThrow(() -> new NullPointerException("Interior를 등록한 적 없습니다."));;
                        interiorRepository.delete(interiorDelete);
                        break;
                    case MOD:
                        Interior interiorUpdate = interiorRepository.findById(reqPutRoomDetail.getInteriorId())
                                .orElseThrow(() -> new NullPointerException("Interior를 등록한 적 없습니다."));;
                        interiorUpdate.updatePlace(
                                reqPutRoomDetail.getX(),
                                reqPutRoomDetail.getY(),
                                reqPutRoomDetail.getZ(),
                                reqPutRoomDetail.getRotX(),
                                reqPutRoomDetail.getRotY(),
                                reqPutRoomDetail.getRotZ()
                        );
                        break;
                }
                interiorRepository.flush();
            }
        }
    }

    @Transactional
    public ResGetInteriorList readInterior(Long id, Long roomId) {
        User user = userRepository.findByIdAndIsDeleted(id, false)
                .orElseThrow(() -> new NullPointerException("User를 찾을 수 없습니다."));
        Room room = roomRepository.findByIdAndUser(roomId, user)
                .orElseThrow(() -> new NullPointerException("Room을 찾을 수 없습니다."));
        List<Interior> interiors = interiorRepository.findByRoom(room);

        List<ResGetInterior> interiorList = new ArrayList<>();

        for(Interior interior: interiors){
            interiorList.add(ResGetInterior.builder()
                            .interiorId(interior.getId())
                            .furnitureId(interior.getFurniture().getId())
                            .resource(interior.getFurniture().getResource())
                            .x(interior.getX())
                            .xRot(interior.getXRot())
                            .y(interior.getY())
                            .yRot(interior.getYRot())
                            .z(interior.getZ())
                            .zRot(interior.getZRot())
                            .build());
        }
        return ResGetInteriorList.builder()
                .interiorList(interiorList)
                .build();
    }
}