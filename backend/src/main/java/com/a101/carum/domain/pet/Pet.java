package com.a101.carum.domain.pet;

import com.a101.carum.domain.question.FaceType;
import com.a101.carum.domain.user.User;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.DynamicInsert;
import org.hibernate.annotations.DynamicUpdate;

import javax.persistence.*;

@Entity
@Getter
@NoArgsConstructor
@AllArgsConstructor
@DynamicUpdate
@DynamicInsert
@Table(name = "pet")
@Builder
public class Pet {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private Long id;

    @ManyToOne(targetEntity = User.class ,fetch = FetchType.LAZY)
    @JoinColumn(name = "user_id", referencedColumnName = "id")
    private User user;

    @Column(name = "year")
    private Long year;

    @Column(name = "month")
    private Long month;

    @Column(name = "appearance")
    private Integer appearance;

    @Column(name = "face")
    private FaceType face;

    @Column(name = "type")
    private PetType type;

}